using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IUserRatingRepository
    {
        // Task<Result<UserRating>> GetAllUserRatingsAsync(CancellationToken cancellationToken = default);
        Task<Result<UserRating>> GetUserRatingByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating>>> GetAllUserRatingsByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating>>> GetAllUserRatingsToUserIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<UserRating>> CreateUserRatingAsync(UserRating userRating, CancellationToken cancellationToken = default);
        Task<Result<UserRating>> UpdateUserRatingAsync(UserRating userRating, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserRatingAsync(Guid userRatingId, CancellationToken cancellationToken = default);
    }
}
