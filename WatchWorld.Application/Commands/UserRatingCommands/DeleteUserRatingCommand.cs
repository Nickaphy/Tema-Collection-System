using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.UserRatingCommands
{
    public record DeleteUserRatingCommand(
        Guid specificUserRatingId
    )
    {
    }
}
