using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.UserRatingCommands
{
    public record CreateUserRatingCommand(
        Guid ratedToUserId,
        int ratingAmount,
        string description,
        Guid ratedByUserId
    )
    {
    }
}
