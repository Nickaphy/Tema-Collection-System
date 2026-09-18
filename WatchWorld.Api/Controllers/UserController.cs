using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers(CancellationToken ct)
    {
        var users = await _userUseCase.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var command = new CreateUserCommand(
            firstName: request.firstName,
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
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult> DeleteUser(DeleteUserRequest request, CancellationToken ct)
    {
        var command = new DeleteUserCommand(
            userId: request.userId
        );
        await _userUseCase.DeleteUserAsync(command, ct);
        return NoContent();
    }

    [HttpPut("{userId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<User>> UpdateUser(Guid userId, UpdateUserRequest request, CancellationToken ct)
    {
        var command = new UpdateUserCommand(
            id: request.id,
            firstName: request.firstName,
            lastName: request.lastName,
            phoneNumber: request.phoneNumber,
            email: request.email,
            address: request.address,
            city: request.city,
            note: request.note,
            password: request.password,
            isAdmin: false,
            rating: request.rating
        );
        var user = await _userUseCase.UpdateUserAsync(command, ct);
        return Ok(user);
    }
    
    [HttpPatch("{userId}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> SetAdminStatus(Guid userId, [FromBody] SetAdminRoleRequest request, CancellationToken ct)
    {
        var command = new SetAdminRoleCommand(
            id : userId,
            isAdmin: request.isAdmin

            );
        await _userUseCase.SetAdminRoleAsync(command, ct);
        return NoContent();
    }
}
