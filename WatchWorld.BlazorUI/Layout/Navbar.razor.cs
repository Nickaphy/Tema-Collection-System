using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Radzen;
using WatchWorld.BlazorUI.Dialogs;
using WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO;
using WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO;
using WatchWorld.BlazorUI.Services;

namespace WatchWorld.BlazorUI.Layout
{
    public partial class Navbar : ComponentBase, IDisposable
    {
        [Inject] private WatchWorldApiClient ApiClient { get; set; } = default!;
        [Inject] private ILogger<Navbar> Logger { get; set; } = default!;
        [Inject] private CurrentUserState CurrentUser { get; set; } = default!;

        private List<WatchDto> allWatches = new();
        private List<WatchDto> searchResults = new();
        private string searchQuery = string.Empty;
        private bool showResults;

        protected override async Task OnInitializedAsync()
        {
            CurrentUser.Changed += OnCurrentUserChanged;

            try
            {
                allWatches = await ApiClient.GetWatchesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "NavBar could not preload watches for search");
            }
        }

        private void OnCurrentUserChanged() => InvokeAsync(StateHasChanged);

        private void OnSearchInput(ChangeEventArgs e)
        {
            searchQuery = e.Value?.ToString() ?? string.Empty;
            showResults = true;

            searchResults = string.IsNullOrWhiteSpace(searchQuery)
                ? new()
                : allWatches
                    .Where(w => w.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .Take(8)
                    .ToList();
        }

        private void OnSearchFocusOut(FocusEventArgs e)
        {
            _ = Task.Delay(150).ContinueWith(_ =>
            {
                showResults = false;
                InvokeAsync(StateHasChanged);
            });
        }

        private async Task OpenLoginDialog()
        {
            var result = await DialogService.OpenAsync<LogInDialog>(
                "Log ind",
                options: new DialogOptions { Width = "620px", ShowClose = true });

            if (result is LogInResponseDto login)
                CurrentUser.LogIn(login, login.Token);
        }

        private async Task OpenRegisterDialog()
        {
            await DialogService.OpenAsync<RegisterUserDialog>(
                "Opret bruger",
                options: new DialogOptions { Width = "480px", ShowClose = true });
        }

        private void LogOut()
        {
            CurrentUser.Clear();
            Navigation.NavigateTo("/");
        }

        public void Dispose()
        {
            CurrentUser.Changed -= OnCurrentUserChanged;
        }
    }
}