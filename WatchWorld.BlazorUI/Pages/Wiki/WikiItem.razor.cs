using System.Globalization;
using Microsoft.AspNetCore.Components;
using WatchWorld.BlazorUI.ResponseDTO;

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
        private List<ListingDto> relatedListings = new();
        private int selectedImageIndex;

        private string MainImageUrl =>
            watch is { Images.Count: > 0 } ? watch.Images[Math.Clamp(selectedImageIndex, 0, watch.Images.Count - 1)].Url : PlaceholderImage;

        private string BraceletTypesText =>
            watch is null ? "" : string.Join(", ", watch.BraceletTypeEnum.Select(EnumDisplay.BraceletType));

        // To-Do: There's no GET /api/Watches/by-model/{modelNumber} endpoint, so this resolves by fetching the full list and matching client-side.
        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            errorMessage = null;
            selectedImageIndex = 0;

            try
            {
                var watches = await ApiClient.GetWatchesAsync(CancellationToken.None);
                watch = watches.FirstOrDefault(w => string.Equals(w.ModelNumber, ModelNumber, StringComparison.OrdinalIgnoreCase));

                if (watch is not null)
                {
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