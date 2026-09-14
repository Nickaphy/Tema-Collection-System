using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.UserRatingCommands
{
    public record CreateUserRatingCommand(
        Guid ratedByUserId,
        Guid ratedToUserId,
        int ratingAmount,
        string description
    )
    {
    }
}
