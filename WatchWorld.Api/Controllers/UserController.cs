using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.UserRequests;
using WatchWorld.Api.Extensions;
using WatchWorld.Api.Responses;
using WatchWorld.Application.Commands.UserCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Results;
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
        var result = await _userUseCase.GetAllUsersAsync();

        if (result.IsFailed)
            return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

        return Ok(result.Value.Select(UserResponse.FromUser));
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        // Shows exactly what the API knows about the caller, read from their token
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
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
        return CreatedAtAction(nameof(GetAllUsers), new { id = user.Value.Id }, user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LogInResult>> LogIn([FromBody] LogInRequest request, CancellationToken ct)
    {
        var command = new LogInCommand(
            firstName: request.firstName,
            lastName: request.lastName,
            email: request.email,
            password: request.password
        );

        var result = await _userUseCase.LogInAsync(command, ct);

        if (result.IsFailed)
            return Unauthorized(string.Join("; ", result.Errors.Select(e => e.Message)));

        return Ok(result.Value);
    }

    [HttpDelete("{userId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult> DeleteUser(Guid userId, CancellationToken ct)
    {
        // Only the user themself, or an admin, may delete this account
        if (User.GetUserId() != userId && !User.IsAdmin())
            return Forbid();

        var command = new DeleteUserCommand(
            userId: userId
        );
        var result = await _userUseCase.DeleteUserAsync(command, ct);

        if (result.IsFailed)
            return NotFound(string.Join("; ", result.Errors.Select(e => e.Message)));

        return NoContent();
    }

    [HttpPut("{userId}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid userId, UpdateUserRequest request, CancellationToken ct)
    {
        // Only the user themself, or an admin, may change this account
        if (User.GetUserId() != userId && !User.IsAdmin())
            return Forbid();

        var command = new UpdateUserCommand(
            id: userId,                 // CHANGED: from the URL, not from the body
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
        var result = await _userUseCase.UpdateUserAsync(command, ct);

        if (result.IsFailed)
            return BadRequest(string.Join("; ", result.Errors.Select(e => e.Message)));

        return Ok(UserResponse.FromUser(result.Value));
    }
    
    [HttpPatch("{userId}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> SetAdminStatus(Guid userId, [FromBody] SetAdminRoleRequest request, CancellationToken ct)
    {
        var command = new SetAdminRoleCommand(
            userId : userId,
            isAdmin: request.isAdmin

            );
        await _userUseCase.SetAdminRoleAsync(command, ct);
        return NoContent();
    }


}
