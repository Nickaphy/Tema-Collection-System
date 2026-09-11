using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IUserRatingRepository
    {
        Task<Result<UserRating?>> GetUserRatingByIdAsync(Guid specificUserRatingId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsByRatedByUserIdAsync(CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsByRatedToUserIdAsync(Guid ratedToUserId, CancellationToken cancellationToken = default);>
        Task<Result<UserRating>> CreateUserRatingAsync(UserRating userRating, CancellationToken ct = default);
        Task<Result<UserRating>> UpdateUserRatingAsync(UserRating userRating, CancellationToken ct = default);
        Task<Result> DeleteUserRatingAsync(Guid specificUserRatingId, CancellationToken cancellationToken = default);
    }
}
