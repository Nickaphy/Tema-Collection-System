using WatchWorld.Domain.Entities;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Api.Requests.UserRatingRequests
{
    public record CreateUserRatingRequest(
        Guid ratedToUserId,
        int ratingAmount,
        string description,
        Guid ratedByUserId
    )
    {
    }
}