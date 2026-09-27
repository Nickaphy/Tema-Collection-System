namespace WatchWorld.Application.Commands.UserCommands
{
    public record LogInCommand(string? email, string? firstName, string? lastName, string password)
    {
    }
}
