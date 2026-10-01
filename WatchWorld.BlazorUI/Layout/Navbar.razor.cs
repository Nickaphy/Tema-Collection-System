using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using WatchWorld.BlazorUI.Dialogs;
using WatchWorld.BlazorUI.ResponseDTO;

namespace WatchWorld.BlazorUI.Layout
{
    public partial class Navbar : ComponentBase
    {
        [Inject] private WatchWorldApiClient ApiClient { get; set; } = default!;
        [Inject] private ILogger<Navbar> Logger { get; set; } = default!;
        [Inject] private DialogService _dialogService { get; set; }
        private List<WatchDto> allWatches = new();
        private List<WatchDto> searchResults = new();
        private string searchQuery = string.Empty;
        private bool showResults;
        private UserDto? loggedInUser;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                allWatches = await ApiClient.GetWatchesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "NavBar could not preload watches for search");
            }
        }

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
            var result = await _dialogService.OpenAsync<LogInDialog>(
                    "Log ind",
                    options: new DialogOptions { Width = "620px" });

            if (result is UserDto user)
            {
                loggedInUser = user;
                StateHasChanged();
            }
        }

        private async Task OpenRegisterDialog()
        {
            await _dialogService.OpenAsync<RegisterUserDialog>(
                "Opret bruger",
                options: new DialogOptions { Width = "480px", ShowClose = true });
        }

        private void LogOut()
        {
            loggedInUser = null;
        }
    }
}
