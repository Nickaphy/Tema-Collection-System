using Microsoft.AspNetCore.Components;
using WatchWorld.BlazorUI.ResponseDTO.BrandResponseDto;

namespace WatchWorld.BlazorUI.Pages.Brand
{
    public partial class Brandspage : ComponentBase
    {
        private bool isLoading = true;
        private string? errorMessage;
        private string searchQuery = string.Empty;
        private Dictionary<Guid, int> watchCounts = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var watchesTask = ApiClient.GetWatchesAsync(CancellationToken.None);
                await Task.WhenAll(Brands.EnsureLoadedAsync(), watchesTask);

                watchCounts = watchesTask.Result
                    .GroupBy(w => w.BrandId)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente mærker lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load brands page");
            }
            finally
            {
                isLoading = false;
            }
        }

        private void OnSearchInput(ChangeEventArgs e) => searchQuery = e.Value?.ToString() ?? string.Empty;

        private IEnumerable<BrandDto> FilteredBrands
        {
            get
            {
                var q = searchQuery.Trim();
                return string.IsNullOrEmpty(q)
                    ? Brands.All
                    : Brands.All.Where(b =>
                        b.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        b.CountryOfOrigin.Contains(q, StringComparison.OrdinalIgnoreCase));
            }
        }

        private record BrandGroup(string Letter, List<BrandDto> Brands);

        // '#' (names starting with a digit/symbol) sorts last.
        private List<BrandGroup> Groups => FilteredBrands
            .GroupBy(b => FirstLetter(b.Name))
            .OrderBy(g => g.Key == "#" ? "\uffff" : g.Key, StringComparer.Ordinal)
            .Select(g => new BrandGroup(g.Key, g.OrderBy(b => b.Name, StringComparer.OrdinalIgnoreCase).ToList()))
            .ToList();

        private static string FirstLetter(string name)
        {
            var c = name.Trim().FirstOrDefault();
            return char.IsLetter(c) ? char.ToUpperInvariant(c).ToString() : "#";
        }

        private string WatchCountLabel(Guid brandId)
        {
            var count = watchCounts.GetValueOrDefault(brandId);
            return count == 1 ? "1 ur" : $"{count} ure";
        }
    }
}
