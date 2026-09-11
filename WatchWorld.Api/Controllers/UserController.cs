using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.UserRequests;
using WatchWorld.Application.Commands.UserCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserUseCase _userUseCase;

    public UserController(IUserUseCase userUseCase)
    {
        _userUseCase = userUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(CancellationToken ct)
    {
        var users = await _userUseCase.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var command = new CreateUserCommand(
            name: request.firstName,
            lastName: request.lastName,
            phoneNumber: request.phoneNumber,
            email: request.email,
            address: request.address,
            city: request.city,
            note: request.note,
            password: request.password,
            isAdmin: request.isAdmin,
            rating: request.rating
        );
        var user = await _userUseCase.CreateUserAsync(command, ct);
        return CreatedAtAction(nameof(GetAllUsers), new { id = new Guid() }, user);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(Guid userId, CancellationToken ct)
    {
        await _userUseCase.DeleteUserAsync(userId, ct);
        return NoContent();
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<User>> UpdateUser(Guid userId, UpdateUserRequest request, CancellationToken ct)
    {
        var command = new UpdateUserCommand(
            userId: request.userId,
            name: request.firstName,
            lastName: request.lastName,
            phoneNumber: request.phoneNumber,
            email: request.email,
            address: request.address,
            city: request.city,
            note: request.note,
            password: request.password,
            isAdmin: request.isAdmin,
            rating: request.rating
        );
        var user = await _userUseCase.UpdateUserAsync(command, ct);
        return Ok(user);
    }
}