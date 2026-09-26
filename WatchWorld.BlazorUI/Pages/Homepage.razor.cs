using System.Globalization;
using Microsoft.AspNetCore.Components;
using WatchWorld.BlazorUI.ResponseDTO;

namespace WatchWorld.BlazorUI.Pages
{
    public partial class Homepage : ComponentBase
    {
        private static readonly CultureInfo DanishCulture = CultureInfo.GetCultureInfo("da-DK");
        private const string PlaceholderImage = "images/mock/placeholder-watch.jpg";

        private readonly CancellationTokenSource cts = new();

        private bool isLoading = true;
        private string? errorMessage;

        private string searchQuery = string.Empty;

        private readonly string[] filters = { "Nyeste", "Pris", "Højest vurderet" };
        private string selectedFilter = "Nyeste";

        private SpotlightModel? Spotlight;
        private EncyclopediaStats Stats = new(0, 0, 0);
        private List<ListingCardModel> FeaturedListings = new();
        private List<RecentAdditionModel> RecentAdditions = new();

        private IEnumerable<ListingCardModel> DisplayedListings => selectedFilter switch
        {
            "Pris" => FeaturedListings.OrderBy(l => l.PricePerDay),
            _ => FeaturedListings
        };

        private static string FormatPrice(decimal price) => price.ToString("C0", DanishCulture);

        private record SpotlightModel(
            string ModelName,
            string ModelNumber,
            string Summary,
            int ReleaseYear,
            int CaseSize,
            string PrimaryImageUrl,
            string SecondaryImageUrl);

        private record ListingCardModel(string WatchName, int Age, string ImageUrl, decimal PricePerDay, string Url);

        private record RecentAdditionModel(string Name, string ModelNumber, string ImageUrl, string Url);

        private record EncyclopediaStats(int WatchModels, int ActiveListings, int Members);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var watchesTask = ApiClient.GetWatchesAsync(cts.Token);
                var listingsTask = ApiClient.GetListingsAsync(cts.Token);
                var usersTask = ApiClient.GetUsersAsync(cts.Token);

                await Task.WhenAll(watchesTask, listingsTask, usersTask);

                var watches = watchesTask.Result;
                var listings = listingsTask.Result;
                var users = usersTask.Result;

                Stats = new EncyclopediaStats(watches.Count, listings.Count, users.Count);

                Spotlight = watches.Count == 0 ? null : BuildSpotlight(watches[0]);

                RecentAdditions = watches
                    .Take(6)
                    .Select(w => new RecentAdditionModel(w.Name, w.ModelNumber, ResolveImage(w.Images), $"/ure/{w.Id}"))
                    .ToList();

                FeaturedListings = listings
                    .Where(l => l.BorrowableWatch?.SpecificWatch is not null)
                    .Select(l => new ListingCardModel(
                        l.BorrowableWatch.SpecificWatch.Name,
                        l.BorrowableWatch.Age,
                        ResolveImage(l.BorrowableWatch.Picture?.Count > 0 ? l.BorrowableWatch.Picture : l.BorrowableWatch.SpecificWatch.Images),
                        l.PricePerDay,
                        $"/udlaan/{l.Id}"))
                    .ToList();
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente forsidens data lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load WatchWorld frontpage data");
            }
            finally
            {
                isLoading = false;
            }
        }

        private static SpotlightModel BuildSpotlight(WatchDto watch)
        {
            var images = watch.Images ?? new();
            return new(
                ModelName: watch.Name,
                ModelNumber: watch.ModelNumber,
                Summary: string.IsNullOrWhiteSpace(watch.Description) ? "Beskrivelse følger snart." : watch.Description!,
                ReleaseYear: watch.ReleaseYear.Year,
                CaseSize: watch.CaseSize,
                PrimaryImageUrl: images.Count > 0 ? images[0].Url : PlaceholderImage,
                SecondaryImageUrl: images.Count > 1 ? images[1].Url : PlaceholderImage);
        }

        private static string ResolveImage(List<HighResImageDto>? images) =>
            images is { Count: > 0 } ? images[0].Url : PlaceholderImage;

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

}
