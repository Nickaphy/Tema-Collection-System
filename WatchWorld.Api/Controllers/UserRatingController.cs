using FluentResults;
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

    [HttpGet("by-user/{userId}")] // match the openapi.yaml path
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserRating>>> GetAllUserRatingsByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var result = await _userRatingUseCase.GetAllUserRatingsByUserIdAsync(userId, ct);
        if (result.IsFailed)
            return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));
        return Ok(result.Value);
    }

    
    [HttpGet("to-user/{userId}")] // match the openapi.yaml path
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserRating>>> GetAllUserRatingsToUserIdAsync(Guid userId, CancellationToken ct)
    {
        var result = await _userRatingUseCase.GetAllUserRatingsToUserIdAsync(userId, ct);
        if (result.IsFailed)
            return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));
        return Ok(result.Value);
    }



    [HttpGet("{id}")] // match the openapi.yaml path
    [AllowAnonymous]
    public async Task<ActionResult<UserRating>> GetUserRatingByIdAsync(Guid Id, CancellationToken ct)
    {
        //route template is "id" while we had "specificUserRatingId" 
        var result = await _userRatingUseCase.GetUserRatingByIdAsync(Id, ct); 
        if (result.IsFailed)
            return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));
        if (result.Value is null)
            return NotFound();
        return Ok(result.Value);
    }



    [HttpDelete("{id}")]
    //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
    public async Task<ActionResult> DeleteUserRating(Guid id, CancellationToken ct)
    {
        var command = new DeleteUserRatingCommand(
            specificUserRatingId: id
        );
        await _userRatingUseCase.DeleteUserRatingAsync(command, ct);
        return NoContent();
    }



    [HttpPost]
    //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
    public async Task<ActionResult> CreateUserRating(CreateUserRatingRequest request, CancellationToken ct)
    {
        var command = new CreateUserRatingCommand(
            ratedToUserId: request.ratedToUserId,
            ratedByUserId: request.ratedByUserId,
            ratingAmount: request.ratingAmount,
            isRatingWatch: request.isRatingWatch,
            description: request.description
        );
      var result = await _userRatingUseCase.CreateUserRatingAsync(command, ct);
        if (result.IsFailed)
            return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

        return CreatedAtAction(
            nameof(GetUserRatingByIdAsync),
            new { id = result.Value.Id },
            result.Value);
    }



    [HttpPut("{id}")]
    //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
   public async Task<ActionResult> UpdateUserRating(
    Guid id,
    UpdateUserRatingRequest request,
    CancellationToken ct)
{
    var command = new UpdateUserRatingCommand(
        specificUserRatingId: id,
        ratingAmount: request.ratingAmount,
        description: request.description
    );
    await _userRatingUseCase.UpdateUserRatingAsync(command, ct);
    return NoContent();
}
}