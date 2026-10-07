using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using WatchWorld.BlazorUI.Dialogs;
using WatchWorld.BlazorUI.ResponseDTO;
using WatchWorld.BlazorUI.ResponseDTO.ListingResponseDto;
using WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO;
using WatchWorld.Domain.Entities;
using static WatchWorld.BlazorUI.Pages.Facetgroup;

namespace WatchWorld.BlazorUI.Pages
{
    public partial class LoanPage : ComponentBase
    {
        private static readonly CultureInfo DanishCulture = CultureInfo.GetCultureInfo("da-DK");
        private const string PlaceholderImage = "/images/seed/OnTheWay.png";
        private const int PageSize = 12;

        private List<ListingDto> allListings = new();
        private Dictionary<Guid, UserDto> ownerLookup = new();
        private bool isLoading = true;
        private string? errorMessage;

        private string searchQuery = string.Empty;
        private bool showMyListings;
        private int visibleCount = PageSize;

        private readonly HashSet<string> selectedBrands = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedWearGrades = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedMovementTypes = new(StringComparer.OrdinalIgnoreCase);
        private decimal? minPrice;
        private decimal? maxPrice;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            isLoading = true;
            errorMessage = null;

            try
            {
                var listingsTask = ApiClient.GetListingsAsync(CancellationToken.None);
                var usersTask = ApiClient.GetUsersAsync(CancellationToken.None);
                await Task.WhenAll(listingsTask, usersTask, Brands.EnsureLoadedAsync());

                allListings = listingsTask.Result.Where(l => l.BorrowableWatch?.SpecificWatch is not null).ToList();
                ownerLookup = usersTask.Result.ToDictionary(u => u.Id, u => u);
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente udlån lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load udlaan listings");
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SetView(bool mine)
        {
            showMyListings = mine;
            ResetPaging();
        }

        private string OwnerLabel(ListingDto listing) =>
            ownerLookup.TryGetValue(listing.BorrowableWatch.UserId, out var owner)
                ? $"{owner.FirstName} i {owner.City}"
                : "Ukendt ejer";

        private static string ResolveListingImage(ListingDto listing) =>
            listing.BorrowableWatch.Picture.Count > 0 ? listing.BorrowableWatch.Picture[0].Url : PlaceholderImage;

        private void OnSearchInput(ChangeEventArgs e)
        {
            searchQuery = e.Value?.ToString() ?? string.Empty;
            ResetPaging();
        }

        private void ToggleFacet(HashSet<string> set, string value)
        {
            if (!set.Add(value)) set.Remove(value);
            ResetPaging();
        }

        private void ResetPaging() => visibleCount = PageSize;

        private void ClearAllFilters()
        {
            searchQuery = string.Empty;
            selectedBrands.Clear();
            selectedWearGrades.Clear();
            selectedMovementTypes.Clear();
            minPrice = maxPrice = null;
            ResetPaging();
        }

        private IEnumerable<ListingDto> ApplyFilters(IEnumerable<ListingDto> source, string? exceptCategory)
        {
            var result = source;

            if (showMyListings && CurrentUser.User is not null)
                result = result.Where(l => l.BorrowableWatch.UserId == CurrentUser.User.Id);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim();
                result = result.Where(l =>
                    l.BorrowableWatch.SpecificWatch.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    Brands.NameOf(l.BorrowableWatch.SpecificWatch.BrandId).Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    l.BorrowableWatch.SpecificWatch.ModelNumber.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue) result = result.Where(l => l.PricePerDay >= minPrice.Value);
            if (maxPrice.HasValue) result = result.Where(l => l.PricePerDay <= maxPrice.Value);

            if (exceptCategory != "brand" && selectedBrands.Count > 0)
                result = result.Where(l => selectedBrands.Contains(Brands.NameOf(l.BorrowableWatch.SpecificWatch.BrandId)));

            if (exceptCategory != "wearGrade" && selectedWearGrades.Count > 0)
                result = result.Where(l => selectedWearGrades.Contains(EnumDisplayConverter.WearGrade(l.BorrowableWatch.WearGrade)));

            if (exceptCategory != "movementType" && selectedMovementTypes.Count > 0)
                result = result.Where(l => selectedMovementTypes.Contains(EnumDisplayConverter.MovementType(l.BorrowableWatch.SpecificWatch.MovementTypeEnum)));

            return result;
        }

        private List<ListingDto> SortedListings => ApplyFilters(allListings, exceptCategory: null).ToList();
        private List<ListingDto> DisplayedListings => SortedListings.Take(visibleCount).ToList();

        private List<FacetOption> BuildFacet(string category, Func<ListingDto, string> valueSelector)
        {
            var scoped = ApplyFilters(allListings, exceptCategory: category);
            return scoped
                .Select(valueSelector)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .GroupBy(v => v, StringComparer.OrdinalIgnoreCase)
                .Select(g => new FacetOption(g.Key, g.Count()))
                .OrderByDescending(f => f.Count)
                .ThenBy(f => f.Value, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private List<FacetOption> BrandFacet => BuildFacet("brand", l => Brands.NameOf(l.BorrowableWatch.SpecificWatch.BrandId));
        private List<FacetOption> WearGradeFacet => BuildFacet("wearGrade", l => EnumDisplayConverter.WearGrade(l.BorrowableWatch.WearGrade));
        private List<FacetOption> MovementTypeFacet => BuildFacet("movementType", l => EnumDisplayConverter.MovementType(l.BorrowableWatch.SpecificWatch.MovementTypeEnum));

        private record ChipItem(string Label, Action Remove);

        private List<ChipItem> ActiveChips
        {
            get
            {
                var chips = new List<ChipItem>();
                foreach (var v in selectedBrands) chips.Add(new($"Mærke: {v}", () => ToggleFacet(selectedBrands, v)));
                foreach (var v in selectedWearGrades) chips.Add(new($"Slidgrad: {v}", () => ToggleFacet(selectedWearGrades, v)));
                foreach (var v in selectedMovementTypes) chips.Add(new($"Værk: {v}", () => ToggleFacet(selectedMovementTypes, v)));

                if (minPrice.HasValue || maxPrice.HasValue)
                    chips.Add(new($"Pris: {minPrice}-{maxPrice} kr.", () => { minPrice = null; maxPrice = null; }));

                return chips;
            }
        }

        private async Task OpenBorrowDialog(ListingDto listing)
        {
            var parameters = new Dictionary<string, object> { ["Listing"] = listing };
            var result = await DialogService.OpenAsync<BorrowDialog>(
                "Lån dette ur",
                parameters,
                options: new DialogOptions { Width = "480px", ShowClose = true });

            if (result is true)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Forespørgsel sendt",
                    Detail = "Din låneforespørgsel er sendt af sted.",
                    Duration = 4000
                });
            }
        }

        private async Task OpenCreateListingDialog()
        {
            var result = await DialogService.OpenAsync<ListingDialog>(
                "Opret annonce",
                options: new DialogOptions { Width = "480px", ShowClose = true });

            if (result is true)
                await LoadData();
        }

        private async Task DeleteListing(ListingDto listing)
        {
            var (success, error) = await ApiClient.DeleteListingAsync(listing.Id, CancellationToken.None);

            if (!success)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Kunne ikke slette",
                    Detail = error ?? "Der skete en ukendt fejl.",
                    Duration = 4000
                });
                return;
            }

            allListings.Remove(listing);
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Annonce slettet",
                Duration = 3000
            });
        }
    }
}