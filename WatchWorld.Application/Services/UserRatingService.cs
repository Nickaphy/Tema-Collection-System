using FluentResults;
using WatchWorld.Application.Commands.UserRatingCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Service;

namespace WatchWorld.Application.Services;

public class UserRatingService : IUserRatingUseCase
{
    private readonly IUserRatingRepository _userRatingRepository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);
    public UserRatingService(IUserRatingRepository userRatingRepository)
    {
        _userRatingRepository = userRatingRepository;
    }
    public async Task<Result<UserRating>> CreateUserRatingAsync(CreateUserRatingCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var userRating = UserRating.Create(
                ratedToUserId: command.ratedToUserId,
                ratingAmount: command.ratingAmount,
                description: command.description,
                isRatingWatch: command.isRatingWatch,
                ratedByUserId: command.ratedByUserId
            );
            await _userRatingRepository.CreateUserRatingAsync(command, ct);
            return Result.Ok(userRating);
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result<UserRating>> UpdateUserRatingAsync(UpdateUserRatingCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var userRating = await _userRatingRepository.GetUserRatingByIdAsync(command.specificUserRatingId, ct);
            if (userRating.IsFailed)
            {
                return Result.Fail("User rating not found.");
            }
            var updatedUserRating = UserRating.Update(
                specificUserRatingId: command.specificUserRatingId,
                ratingAmount: command.ratingAmount,
                description: command.description
            );
            await _userRatingRepository.UpdateUserRatingAsync(command, ct);
            return Result.Ok(updatedUserRating);
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result> DeleteUserRatingAsync(DeleteUserRatingCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var userRating = await _userRatingRepository.GetUserRatingByIdAsync(command.specificUserRatingId, ct);
            if (userRating.IsFailed)
            {
                return Result.Fail("User rating not found.");
            }
            await _userRatingRepository.DeleteUserRatingAsync(command, ct);
            return Result.Ok();
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result<UserRating?>> GetUserRatingByIdAsync(Guid userRatingId, CancellationToken ct = default)
    {
        var userRating = await _userRatingRepository.GetUserRatingByIdAsync(userRatingId, ct);
        if (userRating.IsFailed)
        {
            return Result.Fail("User rating not found.");
        }
        return Result.Ok(userRating.Value);
    }
    public async Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var userRatings = await _userRatingRepository.GetAllUserRatingsByUserIdAsync(userId, ct);
        return Result.Ok(userRatings.Value);
    }
    public async Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsToUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var userRatings = await _userRatingRepository.GetAllUserRatingsToUserIdAsync(userId, ct);
        return Result.Ok(userRatings.Value);
    }

}