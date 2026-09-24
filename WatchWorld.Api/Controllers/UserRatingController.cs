using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.UserRatingRequests;
using WatchWorld.Application.Commands.UserRatingCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRatingController : ControllerBase
{
    private readonly IUserRatingUseCase _userRatingUseCase;
    public UserRatingController(IUserRatingUseCase userRatingUseCase)
    {
        _userRatingUseCase = userRatingUseCase;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserRating>>> GetAllUserRatingsByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var ratingsBySpecificUser = await _userRatingUseCase.GetAllUserRatingsByUserIdAsync(userId, ct);
        return Ok(ratingsBySpecificUser);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserRating>>> GetAllUserRatingsToUserIdAsync(Guid userId, CancellationToken ct)
    {
        var ratingsToSpecificUsers = await _userRatingUseCase.GetAllUserRatingsToUserIdAsync(userId, ct);
        return Ok(ratingsToSpecificUsers);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<UserRating>> GetUserRatingByIdAsync(Guid specificUserRatingId, CancellationToken ct)
    {
        await _userRatingUseCase.GetUserRatingByIdAsync(specificUserRatingId, ct);
        if (specificUserRatingId == Guid.Empty)
        {
            return NotFound();
        }
        return Ok(specificUserRatingId);
    }
    [HttpDelete]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult> DeleteUserRating(DeleteUserRatingRequest request, CancellationToken ct)
    {
        var command = new DeleteUserRatingCommand(
            specificUserRatingId: request.specificUserRatingId
        );
        await _userRatingUseCase.DeleteUserRatingAsync(command, ct);
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult> CreateUserRating(CreateUserRatingRequest request, CancellationToken ct)
    {
        var command = new CreateUserRatingCommand(
            ratedToUserId: request.ratedToUserId,
            ratedByUserId: request.ratedByUserId,
            ratingAmount: request.ratingAmount,
            description: request.description
        );
        await _userRatingUseCase.CreateUserRatingAsync(command, ct);
        return CreatedAtAction(nameof(CreateUserRating), new { id = new Guid() }, request.ratingAmount);
    }
    [HttpPut]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult> UpdateUserRating(UpdateUserRatingRequest request, CancellationToken ct)
    {
        var command = new UpdateUserRatingCommand(
            specificUserRatingId: request.specificUserRatingId,
            ratingAmount: request.ratingAmount,
            description: request.description
        );
        await _userRatingUseCase.UpdateUserRatingAsync(command, ct);
        return NoContent();
    }
}