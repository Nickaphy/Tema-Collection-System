using Microsoft.AspNetCore.Components;
using WatchWorld.BlazorUI.ResponseDTO.BrandResponseDto;
using WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO;

namespace WatchWorld.BlazorUI.Pages.Brand
{
    public partial class BrandItem : ComponentBase
    {
        private const string PlaceholderImage = "images/mock/placeholder-watch.jpg";
        private const int PageSize = 12;

        [Parameter] public string Slug { get; set; } = string.Empty;

        private bool isLoading = true;
        private string? errorMessage;
        private BrandDto? brand;
        private List<WatchDto> watches = new();
        private Dictionary<Guid, int> watchCounts = new();

        private string sortOption = "year-desc";
        private string? selectedStyle;
        private int visibleCount = PageSize;

        // Re-runs when navigating between brands (same component instance, new Slug).
        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            errorMessage = null;
            brand = null;
            watches = new();
            selectedStyle = null;
            visibleCount = PageSize;

            try
            {
                await Brands.EnsureLoadedAsync();
                brand = Brands.FindBySlug(Slug);

                if (brand is not null)
                {
                    // Filtered client-side from the full list rather than calling the new
                    // by-brand endpoint - see the note on its route/verb in the reply. The
                    // full list is also what gives the "Andre mærker" counts.
                    var all = await ApiClient.GetWatchesAsync(CancellationToken.None);
                    watches = all.Where(w => w.BrandId == brand.Id).ToList();
                    watchCounts = all.GroupBy(w => w.BrandId).ToDictionary(g => g.Key, g => g.Count());
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente mærket lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load brand page for slug {Slug}", Slug);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void SelectStyle(string? style)
        {
            selectedStyle = style;
            visibleCount = PageSize;
        }

        private record StyleChip(string Style, int Count);

        // The watch "Style" (Chronograph, Dress, GMT ...) plays the role of Fragrantica's
        // per-brand "Collections" chips.
        private List<StyleChip> StyleChips => watches
            .Where(w => !string.IsNullOrWhiteSpace(w.Style))
            .GroupBy(w => w.Style, StringComparer.OrdinalIgnoreCase)
            .Select(g => new StyleChip(g.Key, g.Count()))
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.Style, StringComparer.OrdinalIgnoreCase)
            .ToList();

        private List<WatchDto> FilteredWatches
        {
            get
            {
                var result = selectedStyle is null
                    ? watches
                    : watches.Where(w => string.Equals(w.Style, selectedStyle, StringComparison.OrdinalIgnoreCase)).ToList();

                return sortOption switch
                {
                    "name-asc" => result.OrderBy(w => w.Name, StringComparer.OrdinalIgnoreCase).ToList(),
                    "price-asc" => result.OrderBy(w => w.OriginalPrice).ToList(),
                    "price-desc" => result.OrderByDescending(w => w.OriginalPrice).ToList(),
                    _ => result.OrderByDescending(w => w.ReleaseYear).ToList()
                };
            }
        }

        private List<WatchDto> DisplayedWatches => FilteredWatches.Take(visibleCount).ToList();

        private record OtherBrand(BrandDto Brand, int Count);

        // Sidebar, like Fragrantica's "Most Popular Brands": the brands with the most watches.
        private List<OtherBrand> OtherBrands => Brands.All
            .Where(b => brand is not null && b.Id != brand.Id)
            .Select(b => new OtherBrand(b, watchCounts.GetValueOrDefault(b.Id)))
            .OrderByDescending(o => o.Count)
            .ThenBy(o => o.Brand.Name, StringComparer.OrdinalIgnoreCase)
            .Take(8)
            .ToList();
    }
}