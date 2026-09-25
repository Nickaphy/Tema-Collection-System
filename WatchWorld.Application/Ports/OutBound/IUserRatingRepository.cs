using FluentResults;
using WatchWorld.Application.Commands.UserRatingCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IUserRatingRepository
    {
        Task<Result<UserRating>> CreateUserRatingAsync(UserRating userRating, CancellationToken cancellationToken = default);
        Task<Result<UserRating>> UpdateUserRatingAsync(UserRating userRating, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserRatingAsync(Guid userRatingId, CancellationToken cancellationToken = default);
        Task<Result<UserRating?>> GetUserRatingByIdAsync(Guid userRatingId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsToUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
