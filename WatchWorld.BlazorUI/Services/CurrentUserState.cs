using WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO;

namespace WatchWorld.BlazorUI.Services
{
    // single shared piece of app state - to control the current logged in user across the app
    public class CurrentUserState
    {
        public UserDto? User { get; private set; }

        // the key card. null = not logged in
        public string? Token { get; private set; }

        public event Action? Changed;

        public void SetUser(UserDto? user)
        {
            User = user;
            Changed?.Invoke();
        }

        // called once, right after a successful login
        public void LogIn(UserDto user, string token)
        {
            Token = token;
            SetUser(user);
        }

        // logging out also throws away the token
        public void Clear()
        {
            Token = null;
            SetUser(null);
        }
    }
}