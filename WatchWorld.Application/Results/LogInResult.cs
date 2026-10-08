namespace WatchWorld.Application.Results
{
    public record LogInResult(
        Guid id,
        string firstName,
        string lastName,
        string phoneNumber,
        string email,
        string address,
        string city,
        string? note,
        bool isAdmin,
        string token //Herfra bruger vi token, ikke password, fordi vi ikke vil have serveren til at sende password ud til brugeren igen.
    )
    {
    }
}