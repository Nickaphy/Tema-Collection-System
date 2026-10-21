using WatchWorld.BlazorUI.Helpers.BrandHelpers;
using WatchWorld.BlazorUI.ResponseDTO;
using WatchWorld.BlazorUI.ResponseDTO.BrandResponseDto;
using WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO;

namespace WatchWorld.BlazorUI.Services
{
    public class BrandCatalogHelper
    {
        private readonly WatchWorldApiClient _api;
        private Dictionary<Guid, BrandDto> _byId = new();
        private Task? _loading;
        private bool _loaded;

        public BrandCatalogHelper(WatchWorldApiClient api)
        {
            _api = api;
        }

        public IReadOnlyList<BrandDto> All { get; private set; } = Array.Empty<BrandDto>();

        public async Task EnsureLoadedAsync()
        {
            if (_loaded) return;

            _loading ??= LoadAsync();

            try
            {
                await _loading;
            }
            catch
            {
                // Don't cache a failed load - let the next caller retry.
                _loading = null;
                throw;
            }
        }

        // Call after creating/editing a brand so the next EnsureLoadedAsync refetches.
        public void Invalidate()
        {
            _loaded = false;
            _loading = null;
        }

        private async Task LoadAsync()
        {
            var brands = await _api.GetBrandsAsync(CancellationToken.None);
            All = brands.OrderBy(b => b.Name, StringComparer.OrdinalIgnoreCase).ToList();
            _byId = brands.ToDictionary(b => b.Id);
            _loaded = true;
        }

        public BrandDto? Get(Guid id) => _byId.GetValueOrDefault(id);

        public string NameOf(Guid id) => _byId.TryGetValue(id, out var brand) ? brand.Name : string.Empty;

        public BrandDto? FindBySlug(string slug)
        {
            var wanted = BrandTextHelper.Slug(slug);
            return All.FirstOrDefault(b => BrandTextHelper.Slug(b.Name) == wanted);
        }
        public string FullName(WatchDto watch)
        {
            var brand = NameOf(watch.BrandId);
            return string.IsNullOrEmpty(brand) || watch.Name.StartsWith(brand, StringComparison.OrdinalIgnoreCase)
                ? watch.Name
                : $"{brand} {watch.Name}";
        }
    }
}
