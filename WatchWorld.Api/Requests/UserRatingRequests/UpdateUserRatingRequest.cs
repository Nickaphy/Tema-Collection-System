using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Requests.UserRatingRequests
{
    public record UpdateUserRatingRequest(
        Guid specificUserRatingId,
        int ratingAmount,
        string description
    )
    {
    }
}
