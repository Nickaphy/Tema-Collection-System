using Microsoft.JSInterop;

namespace WatchWorld.BlazorUI.Helpers
{
    public class LogInContext
    {
        private readonly IJSRuntime _js;

        public LogInContext(IJSRuntime js)
        {
            _js = js;
        }

        public Guid UserId { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public bool IsLoggedIn => UserId != Guid.Empty;

        public event Action? OnChange;

        public async Task SetSessionAsync(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName; // Fixed parameter variable name assignment

            try
            {
                await _js.InvokeVoidAsync("sessionStorage.setItem", "userId", userId.ToString());
                await _js.InvokeVoidAsync("sessionStorage.setItem", "userName", userName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Storage Error] Failed to write session: {ex.Message}");
            }

            OnChange?.Invoke();
        }

        public async Task LoadFromStorageAsync()
        {
            try
            {
                var userId = await _js.InvokeAsync<string?>("sessionStorage.getItem", "userId");
                var userName = await _js.InvokeAsync<string?>("sessionStorage.getItem", "userName");

                if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var uId))
                {
                    UserId = uId;
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    UserName = userName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Storage Error] Failed to load session storage: {ex.Message}");
            }

            OnChange?.Invoke();
        }

        public async Task ClearAsync()
        {
            UserId = Guid.Empty;
            UserName = string.Empty;

            try
            {
                await _js.InvokeVoidAsync("sessionStorage.removeItem", "userId");
                await _js.InvokeVoidAsync("sessionStorage.removeItem", "userName");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Storage Error] Failed to clear session storage: {ex.Message}");
            }

            OnChange?.Invoke();
        }
    }
}
