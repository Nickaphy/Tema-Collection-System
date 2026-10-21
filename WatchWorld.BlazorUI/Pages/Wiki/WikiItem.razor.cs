using System.Globalization;
using Microsoft.AspNetCore.Components;
using WatchWorld.BlazorUI.ResponseDTO;
using WatchWorld.BlazorUI.ResponseDTO.BrandResponseDto;
using WatchWorld.BlazorUI.ResponseDTO.ListingResponseDto;
using WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO;

namespace WatchWorld.BlazorUI.Pages.Wiki
{
    public partial class WikiItem : ComponentBase
    {
        private static readonly CultureInfo DanishCulture = CultureInfo.GetCultureInfo("da-DK");
        private const string PlaceholderImage = "images/mock/placeholder-watch.jpg";

        [Parameter] public string ModelNumber { get; set; } = string.Empty;

        private bool isLoading = true;
        private string? errorMessage;
        private WatchDto? watch;
        private BrandDto? brand;
        private List<ListingDto> relatedListings = new();
        private int selectedImageIndex;

        private string MainImageUrl =>
            watch is { Images.Count: > 0 } ? watch.Images[Math.Clamp(selectedImageIndex, 0, watch.Images.Count - 1)].Url : PlaceholderImage;

        private string BraceletTypesText =>
            watch is null ? "" : string.Join(", ", watch.BraceletTypeEnum.Select(EnumDisplayConverter.BraceletType));

        // NOTE: there's no GET /api/Watches/by-model/{modelNumber} endpoint, so this
        // resolves by fetching the full list and matching client-side. Fine at today's
        // catalog size; worth a dedicated endpoint if the catalog grows a lot.
        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            errorMessage = null;
            selectedImageIndex = 0;
            brand = null;

            try
            {
                var watches = await ApiClient.GetWatchesAsync(CancellationToken.None);
                watch = watches.FirstOrDefault(w => string.Equals(w.ModelNumber, ModelNumber, StringComparison.OrdinalIgnoreCase));

                if (watch is not null)
                {
                    await Brands.EnsureLoadedAsync();
                    brand = Brands.Get(watch.BrandId);

                    var listings = await ApiClient.GetListingsAsync(CancellationToken.None);
                    relatedListings = listings
                        .Where(l => l.BorrowableWatch?.SpecificWatch is not null &&
                                    string.Equals(l.BorrowableWatch.SpecificWatch.ModelNumber, watch.ModelNumber, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente modellen lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load wiki detail for model {ModelNumber}", ModelNumber);
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}