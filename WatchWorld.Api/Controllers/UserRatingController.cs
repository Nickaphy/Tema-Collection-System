using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.UserRequests;
using WatchWorld.Application.Commands.UserCommands;
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
    [HttpDelete]
    public async Task<ActionResult> DeleteUserRating(DeleteUserRatingRequest request, CancellationToken ct)
    {
        var command = new DeleteUserRatingCommand(
            ratedToUserId: request.ratedToUserId,
            ratedByUserId: request.ratedByUserId
        );
        await _userRatingUseCase.DeleteUserRatingAsync(command, ct);
        return NoContent();
    }
}