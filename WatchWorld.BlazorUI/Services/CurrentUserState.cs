using WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO;

namespace WatchWorld.BlazorUI.Services
{
    // single shared piece of app state - to control the current logged in user across the app
    public class CurrentUserState
    {
        public UserDto? User { get; private set; }

        public event Action? Changed;

        public void SetUser(UserDto? user)
        {
            User = user;
            Changed?.Invoke();
        }

        public void Clear() => SetUser(null);
    }

}
