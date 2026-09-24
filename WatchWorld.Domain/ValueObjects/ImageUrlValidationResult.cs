namespace WatchWorld.Domain.ValueObjects
{
    public class ImageUrlValidationResult
    {
        public bool IsValid { get; init; }
        public string? ErrorMessage { get; init; }
        public static ImageUrlValidationResult Success() => new() { IsValid = true };
        public static ImageUrlValidationResult Failure(string message) =>
            new() { IsValid = false, ErrorMessage = message };
    }

}
