using System.Net.Http.Json;
using WatchWorld.BlazorUI.ResponseDTO;

namespace WatchWorld.BlazorUI
{
    public class WatchWorldApiClient
    {
        private readonly HttpClient _http;

        public WatchWorldApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<WatchDto>> GetWatchesAsync(CancellationToken ct) =>
            await _http.GetFromJsonAsync<List<WatchDto>>("api/Watches", ct) ?? new();

        public async Task<List<ListingDto>> GetListingsAsync(CancellationToken ct) =>
            await _http.GetFromJsonAsync<List<ListingDto>>("api/Listing", ct) ?? new();

        public async Task<List<UserDto>> GetUsersAsync(CancellationToken ct) =>
            await _http.GetFromJsonAsync<List<UserDto>>("api/User", ct) ?? new();
    }

}
