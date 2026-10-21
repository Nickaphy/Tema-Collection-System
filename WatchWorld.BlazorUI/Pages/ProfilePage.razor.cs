using Microsoft.AspNetCore.Components;
using Radzen;
using WatchWorld.BlazorUI.Dialogs;
using WatchWorld.BlazorUI.ResponseDTO.IndividualWatchResponseDto;
using WatchWorld.BlazorUI.ResponseDTO.UserRatingDTO;
using WatchWorld.BlazorUI.ResponseDTO.UserRatingResponseDTO;
using WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO;

namespace WatchWorld.BlazorUI.Pages
{
    public partial class ProfilePage : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }

        private bool isLoading = true;
        private string? errorMessage;
        private UserDto? profileUser;
        private List<UserRatingDto> ratings = new();
        private List<IndividualWatchDto> ownedWatches = new();

        private bool IsOwnProfile => CurrentUser.User is not null && CurrentUser.User.Id == Id;

        private string Initials => profileUser is null ? "" :
            $"{(profileUser.FirstName.Length > 0 ? profileUser.FirstName[0] : ' ')}{(profileUser.LastName.Length > 0 ? profileUser.LastName[0] : ' ')}"
                .ToUpperInvariant();

        // Edit form fields (own profile only)
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string phoneNumber = string.Empty;
        private string address = string.Empty;
        private string city = string.Empty;
        private string? note;
        private string password = string.Empty;
        private bool isSaving;
        private string? saveError;

        // New rating form fields (other users' profiles only)
        private int newRatingAmount = 5;
        private string newRatingDescription = string.Empty;
        private bool isSubmittingRating;
        private string? ratingError;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            errorMessage = null;

            try
            {
                // NOTE: there's no GET /api/User/{id} endpoint, so this fetches everyone
                // and filters client-side - fine at today's scale, worth a dedicated
                // endpoint if the user base grows.
                var users = await ApiClient.GetUsersAsync(CancellationToken.None);
                profileUser = users.FirstOrDefault(u => u.Id == Id);

                if (profileUser is not null)
                {
                    firstName = profileUser.FirstName;
                    lastName = profileUser.LastName;
                    email = profileUser.Email;
                    phoneNumber = profileUser.PhoneNumber;
                    address = profileUser.Address;
                    city = profileUser.City;
                    note = profileUser.Note;

                    ratings = await ApiClient.GetUserRatingsReceivedAsync(Id, CancellationToken.None);

                    if (IsOwnProfile)
                    {
                        await Brands.EnsureLoadedAsync();
                        ownedWatches = await ApiClient.GetIndividualWatchesByUserAsync(Id, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Kunne ikke hente profilen lige nu. Prøv at genindlæse siden.";
                Logger.LogError(ex, "Failed to load profile {UserId}", Id);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task OpenAddWatchDialog()
        {
            var result = await DialogService.OpenAsync<AddWatchDialog>(
                "Tilføj ur",
                options: new DialogOptions { Width = "460px", ShowClose = true });

            if (result is true)
                ownedWatches = await ApiClient.GetIndividualWatchesByUserAsync(Id, CancellationToken.None);
        }

        private static string Stars(int amount)
        {
            var clamped = Math.Clamp(amount, 0, 5);
            return new string('★', clamped) + new string('☆', 5 - clamped);
        }

        private async Task SaveChanges()
        {
            saveError = null;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(city))
            {
                saveError = "Udfyld venligst alle påkrævede felter.";
                return;
            }

            // UpdateUser's Validate() requires a password on every call, and UserDto
            // doesn't carry the current one (for good reason). Until there's a separate
            // "change password" flow, re-entering it on every profile save is required.
            if (string.IsNullOrWhiteSpace(password))
            {
                saveError = "Indtast venligst din adgangskode for at gemme ændringer.";
                return;
            }

            isSaving = true;

            var request = new UpdateUserRequestDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                Note = note,
                Password = password
            };

            var (success, updatedUser, error) = await ApiClient.UpdateUserAsync(Id, request, CancellationToken.None);

            isSaving = false;

            if (!success || updatedUser is null)
            {
                saveError = error ?? "Der skete en ukendt fejl. Prøv igen.";
                return;
            }

            profileUser = updatedUser;
            CurrentUser.SetUser(updatedUser);
            password = string.Empty;

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Gemt",
                Detail = "Dine oplysninger er opdateret.",
                Duration = 3000
            });
        }

        private async Task SubmitRating()
        {
            ratingError = null;

            if (CurrentUser.User is null) return;

            if (string.IsNullOrWhiteSpace(newRatingDescription))
            {
                ratingError = "Skriv venligst en beskrivelse.";
                return;
            }

            isSubmittingRating = true;

            var request = new CreateUserRatingRequestDto
            {
                RatedToUserId = Id,
                RatedByUserId = CurrentUser.User.Id,
                RatingAmount = newRatingAmount,
                IsRatingWatch = false,
                Description = newRatingDescription
            };

            var (success, error) = await ApiClient.CreateUserRatingAsync(request, CancellationToken.None);

            isSubmittingRating = false;

            if (!success)
            {
                ratingError = error ?? "Kunne ikke sende anmeldelsen. Prøv igen.";
                return;
            }

            newRatingDescription = string.Empty;
            newRatingAmount = 5;
            ratings = await ApiClient.GetUserRatingsReceivedAsync(Id, CancellationToken.None);
        }
    }
}