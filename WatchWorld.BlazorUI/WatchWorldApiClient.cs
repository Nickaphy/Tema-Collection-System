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

        public async Task<(bool Success, string? Error)> RegisterUserAsync(CreateUserRequestDto request, CancellationToken ct)
        {
            var response = await _http.PostAsJsonAsync("api/User/register", request, ct);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var body = await response.Content.ReadAsStringAsync(ct);
            return (false, string.IsNullOrWhiteSpace(body) ? $"Fejl ({(int)response.StatusCode})" : body);
        }

        public async Task<(bool Success, UserDto? User, string? Error)> LogInAsync(LogInRequestDto request, CancellationToken ct)
        {
            var response = await _http.PostAsJsonAsync("api/User/login", request, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                var message = string.IsNullOrWhiteSpace(errorBody) ? $"Fejl ({(int)response.StatusCode})" : errorBody;
                return (false, null, message);
            }

            var user = await response.Content.ReadFromJsonAsync<UserDto>(cancellationToken: ct);
            return (true, user, null);
        }

    }
}
