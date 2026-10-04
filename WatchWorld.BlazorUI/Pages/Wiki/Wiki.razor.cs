using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using WatchWorld.BlazorUI.Dialogs;
using WatchWorld.BlazorUI.ResponseDTO;
using WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO;
using WatchWorld.Domain.Entities;
using static WatchWorld.BlazorUI.Pages.Facetgroup;

namespace WatchWorld.BlazorUI.Pages.Wiki
{
    public partial class Wiki : ComponentBase
    {
        private const string PlaceholderImage = "images/mock/placeholder-watch.jpg";
        private const int PageSize = 12;

        private List<WatchDto> allWatches = new();
        private bool isLoading = true;
        private string? errorMessage;

        private string searchQuery = string.Empty;
        private string sortOption = "relevance";
        private int visibleCount = PageSize;

        private readonly HashSet<string> selectedBrands = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedStyles = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedCaseShapes = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedCaseMaterials = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedMovementTypes = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedGenders = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> selectedBraceletTypes = new(StringComparer.OrdinalIgnoreCase);

        private int? minCaseSize;
        private int? maxCaseSize;
        private decimal? minPrice;
        private decimal? maxPrice;
        private int? minYear;
        private int? maxYear;

        protected override async Task OnInitializedAsync()
        {
            await LoadWatches();
        }

        private async Task LoadWatches()
        {
            isLoading = true;
            errorMessage = null;

            try
            {
                await Brands.EnsureLoadedAsync();
                allWatches = await ApiClient.GetWatchesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente encyklopædiet lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load watches for the wiki page");
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task OpenCreateWatchDialog()
        {
            var result = await DialogService.OpenAsync<CreateWatchDialog>(
                "Opret urmodel",
                options: new DialogOptions { Width = "640px", ShowClose = true });

            if (result is true)
                await LoadWatches();
        }

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
            selectedStyles.Clear();
            selectedCaseShapes.Clear();
            selectedCaseMaterials.Clear();
            selectedMovementTypes.Clear();
            selectedGenders.Clear();
            selectedBraceletTypes.Clear();
            minCaseSize = maxCaseSize = null;
            minPrice = maxPrice = null;
            minYear = maxYear = null;
            ResetPaging();
        }

        // Applies every active filter except the one named in exceptCategory (if any) -
        // used both for the real result list (exceptCategory: null) and for computing
        // each facet's own option counts (excluding its own selections, so counts show
        // "how many results if you also picked this option" rather than collapsing to
        // the current selection).
        private IEnumerable<WatchDto> ApplyFilters(IEnumerable<WatchDto> source, string? exceptCategory)
        {
            var result = source;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim();
                result = result.Where(w =>
                    w.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    Brands.NameOf(w.BrandId).Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    w.ModelNumber.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (minCaseSize.HasValue) result = result.Where(w => w.CaseSize >= minCaseSize.Value);
            if (maxCaseSize.HasValue) result = result.Where(w => w.CaseSize <= maxCaseSize.Value);
            if (minPrice.HasValue) result = result.Where(w => w.OriginalPrice >= minPrice.Value);
            if (maxPrice.HasValue) result = result.Where(w => w.OriginalPrice <= maxPrice.Value);
            if (minYear.HasValue) result = result.Where(w => w.ReleaseYear.Year >= minYear.Value);
            if (maxYear.HasValue) result = result.Where(w => w.ReleaseYear.Year <= maxYear.Value);

            if (exceptCategory != "brand" && selectedBrands.Count > 0)
                result = result.Where(w => selectedBrands.Contains(Brands.NameOf(w.BrandId)));

            if (exceptCategory != "style" && selectedStyles.Count > 0)
                result = result.Where(w => selectedStyles.Contains(w.Style));

            if (exceptCategory != "caseShape" && selectedCaseShapes.Count > 0)
                result = result.Where(w => selectedCaseShapes.Contains(EnumDisplayConverter.CaseShape(w.CaseShapeEnum)));

            if (exceptCategory != "caseMaterial" && selectedCaseMaterials.Count > 0)
                result = result.Where(w => selectedCaseMaterials.Contains(EnumDisplayConverter.CaseMaterial(w.CaseMaterialEnum)));

            if (exceptCategory != "movementType" && selectedMovementTypes.Count > 0)
                result = result.Where(w => selectedMovementTypes.Contains(EnumDisplayConverter.MovementType(w.MovementTypeEnum)));

            if (exceptCategory != "gender" && selectedGenders.Count > 0)
                result = result.Where(w => selectedGenders.Contains(EnumDisplayConverter.Gender(w.GenderEnum)));

            if (exceptCategory != "braceletType" && selectedBraceletTypes.Count > 0)
                result = result.Where(w => w.BraceletTypeEnum.Any(b => selectedBraceletTypes.Contains(EnumDisplayConverter.BraceletType(b))));

            return result;
        }

        private List<WatchDto> FilteredWatches => ApplyFilters(allWatches, exceptCategory: null).ToList();

        private List<WatchDto> SortedWatches => sortOption switch
        {
            "name-asc" => FilteredWatches.OrderBy(w => w.Name, StringComparer.OrdinalIgnoreCase).ToList(),
            "price-asc" => FilteredWatches.OrderBy(w => w.OriginalPrice).ToList(),
            "price-desc" => FilteredWatches.OrderByDescending(w => w.OriginalPrice).ToList(),
            "year-desc" => FilteredWatches.OrderByDescending(w => w.ReleaseYear).ToList(),
            _ => FilteredWatches
        };

        private List<WatchDto> DisplayedWatches => SortedWatches.Take(visibleCount).ToList();

        private List<FacetOption> BuildFacet(string category, Func<WatchDto, IEnumerable<string>> valuesSelector)
        {
            var scoped = ApplyFilters(allWatches, exceptCategory: category);
            return scoped
                .SelectMany(valuesSelector)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .GroupBy(v => v, StringComparer.OrdinalIgnoreCase)
                .Select(g => new FacetOption(g.Key, g.Count()))
                .OrderByDescending(f => f.Count)
                .ThenBy(f => f.Value, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private List<FacetOption> BrandFacet => BuildFacet("brand", w => new[] { Brands.NameOf(w.BrandId) });
        private List<FacetOption> StyleFacet => BuildFacet("style", w => new[] { w.Style });
        private List<FacetOption> CaseShapeFacet => BuildFacet("caseShape", w => new[] { EnumDisplayConverter.CaseShape(w.CaseShapeEnum) });
        private List<FacetOption> CaseMaterialFacet => BuildFacet("caseMaterial", w => new[] { EnumDisplayConverter.CaseMaterial(w.CaseMaterialEnum) });
        private List<FacetOption> MovementTypeFacet => BuildFacet("movementType", w => new[] { EnumDisplayConverter.MovementType(w.MovementTypeEnum) });
        private List<FacetOption> GenderFacet => BuildFacet("gender", w => new[] { EnumDisplayConverter.Gender(w.GenderEnum) });
        private List<FacetOption> BraceletTypeFacet => BuildFacet("braceletType", w => w.BraceletTypeEnum.Select(EnumDisplayConverter.BraceletType));

        private record ChipItem(string Label, Action Remove);

        private List<ChipItem> ActiveChips
        {
            get
            {
                var chips = new List<ChipItem>();
                foreach (var v in selectedBrands) chips.Add(new($"Mærke: {v}", () => ToggleFacet(selectedBrands, v)));
                foreach (var v in selectedStyles) chips.Add(new($"Stil: {v}", () => ToggleFacet(selectedStyles, v)));
                foreach (var v in selectedCaseShapes) chips.Add(new($"Facon: {v}", () => ToggleFacet(selectedCaseShapes, v)));
                foreach (var v in selectedCaseMaterials) chips.Add(new($"Materiale: {v}", () => ToggleFacet(selectedCaseMaterials, v)));
                foreach (var v in selectedMovementTypes) chips.Add(new($"Værk: {v}", () => ToggleFacet(selectedMovementTypes, v)));
                foreach (var v in selectedGenders) chips.Add(new($"Køn: {v}", () => ToggleFacet(selectedGenders, v)));
                foreach (var v in selectedBraceletTypes) chips.Add(new($"Rem: {v}", () => ToggleFacet(selectedBraceletTypes, v)));

                if (minCaseSize.HasValue || maxCaseSize.HasValue)
                    chips.Add(new($"Størrelse: {minCaseSize}-{maxCaseSize} mm", () => { minCaseSize = null; maxCaseSize = null; }));

                if (minPrice.HasValue || maxPrice.HasValue)
                    chips.Add(new($"Pris: {minPrice}-{maxPrice} kr.", () => { minPrice = null; maxPrice = null; }));

                if (minYear.HasValue || maxYear.HasValue)
                    chips.Add(new($"År: {minYear}-{maxYear}", () => { minYear = null; maxYear = null; }));

                return chips;
            }
        }
    }
}