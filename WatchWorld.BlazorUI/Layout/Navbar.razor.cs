using WatchWorld.BlazorUI.ChildComponents;
using WatchWorld.BlazorUI.ResponseDTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace WatchWorld.BlazorUI.Layout
{
    public partial class Navbar : ComponentBase
    {
        [Inject] private WatchWorldApiClient ApiClient { get; set; } = default!;
        [Inject] private DialogService DialogService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private ILogger<Navbar> Logger { get; set; } = default!;

        private List<WatchDto> allWatches = new();
        private List<WatchDto> searchResults = new();
        private string searchQuery = string.Empty;
        private bool showResults;

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
            showResults = !string.IsNullOrWhiteSpace(searchQuery);

            searchResults = string.IsNullOrWhiteSpace(searchQuery)
                ? new()
                : allWatches
                    .Where(w => w.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                                || (w.ModelNumber != null && w.ModelNumber.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
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
            await DialogService.OpenAsync<LogInComponent>(
                "Log ind",
                options: new DialogOptions { Width = "420px", ShowClose = true, Resizable = false });
        }

        private async Task OpenRegisterDialog()
        {
            await DialogService.OpenAsync<RegisterUserComponent>(
                "Opret bruger",
                options: new DialogOptions { Width = "500px", ShowClose = true, Resizable = false });
        }
    }
}
