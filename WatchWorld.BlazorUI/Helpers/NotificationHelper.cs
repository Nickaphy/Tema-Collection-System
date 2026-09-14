namespace WatchWorld.BlazorUI.Helpers
{
    public class NotificationHelper //Stores information about the current notification, such as the message, whether it is a success or error message and whether it is visible.
    {
        public string? Message { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsVisible { get; private set; }

        public event Action? OnChange;

        public void ShowSuccess(string message)
        {
            Message = message;
            IsSuccess = true;
            IsVisible = true;
            OnChange?.Invoke();
        }

        public void ShowError(string message)
        {
            Message = message;
            IsSuccess = false;
            IsVisible = true;
            OnChange?.Invoke();
        }

        public void Clear()
        {
            Message = null;
            IsVisible = false;
            OnChange?.Invoke();
        }
    }
}

