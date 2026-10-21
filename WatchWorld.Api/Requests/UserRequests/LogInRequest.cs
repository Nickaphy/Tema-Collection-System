namespace WatchWorld.Api.Requests.UserRequests
{
    public record LogInRequest (string? email, string? firstName, string? lastName, string password)
    {
    }
}
