using FluentResults;
using WatchWorld.Application.Commands.UserRatingCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IUserRatingUseCase
    {
        Task<Result<UserRating>> CreateUserRatingAsync(CreateUserRatingCommand command, CancellationToken cancellationToken = default);
        Task<Result<UserRating>> UpdateUserRatingAsync(UpdateUserRatingCommand command, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserRatingAsync(DeleteUserRatingCommand command, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsAsync(CancellationToken cancellationToken = default);>
    }
}
