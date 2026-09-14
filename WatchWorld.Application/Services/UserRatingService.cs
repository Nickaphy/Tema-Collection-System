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
        //var userRating = await _userRatingRepository.CreateUserRatingAsync(ct);

        await _Lock.WaitAsync();
        try
        {
            try
            {
                var userRating = UserRating.Create(
                    ratedByUserId: command.ratedByUserId,
                    ratedToUserId: command.ratedToUserId,
                    ratingAmount: command.ratingAmount,
                    description: command.description
                );
                await _userRatingRepository.CreateUserRatingAsync(userRating, ct);
                return Result.Ok(userRating);
            }
        }
    }
}