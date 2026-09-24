namespace WatchWorld.Application.Commands.UserCommands
{
    public record SetAdminRoleCommand (Guid userId, bool isAdmin)
    {
    }
}
