using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Requests.UserRatingRequests
{
    public record DeleteUserRatingRequest(
        Guid specificUserRatingId
    )
    {
    }
}
