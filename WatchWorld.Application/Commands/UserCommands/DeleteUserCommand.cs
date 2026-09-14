using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.UserCommands
{
    public record DeleteUserCommand(
        Guid userId
    )
    {
    }
}
