using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.UserRatingCommands
{
    public record UpdateUserRatingCommand(
        Guid specificUserRatingId,
        int ratingAmount,
        string description
    )
    {
    }
}
