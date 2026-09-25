using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using WatchWorld.Api.Requests.WatchRequests;
using WatchWorld.Application.Commands.WatchesCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchesController : ControllerBase
{
    private readonly IWatchesUseCase _watchUseCase;

    public WatchesController(IWatchesUseCase watchUseCase)
    {
        _watchUseCase = watchUseCase;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Watches>>> Get(CancellationToken ct)
    {
        var watches = await _watchUseCase.GetAllAsync(ct);
        if (watches.IsFailed)
            return Problem(string.Join("; ", watches.Errors.Select(e => e.Message)));
        return Ok(watches.Value);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Watches>> GetById(Guid id, CancellationToken ct)
    {
        var watch = await _watchUseCase.GetWatchByIdAsync(id, ct);
        return Ok(watch);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Watches>> Create([FromBody] CreateWatchRequest request, CancellationToken ct)
    {
        var command = new CreateWatchCommand(
            name: request.name,
            modelNumber: request.modelNumber,
            caseSize: request.caseSize,
            caseShapeEnum: request.caseShapeEnum,
            caseMaterialEnum: request.caseMaterialEnum,
            movementTypeEnum: request.movementTypeEnum,
            style: request.style,
            originalPrice: request.originalPrice,
            genderEnum: request.genderEnum,
            releaseYear: request.releaseYear,
            braceletTypeEnum: request.braceletTypeEnum,
            description: request.description,
            images: request.images
        );

        var watch = await _watchUseCase.CreateWatchAsync(command, ct);
        return CreatedAtAction(nameof(Get), new { id = new Guid() }, watch);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteWatch(DeleteWatchRequest request, CancellationToken ct)
    {
        await _watchUseCase.DeleteWatchAsync(new DeleteWatchCommand(request.watchId), ct);
        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<Watches>> UpdateWatch(UpdateWatchRequest request, CancellationToken ct)
    {
        var command = new UpdateWatchCommand(
            id: request.id,
            name: request.name,
            modelNumber: request.modelNumber,
            caseSize: request.caseSize,
            caseShapeEnum: request.caseShapeEnum,
            caseMaterialEnum: request.caseMaterialEnum,
            movementTypeEnum: request.movementTypeEnum,
            style: request.style,
            originalPrice: request.originalPrice,
            genderEnum: request.genderEnum,
            releaseYear: request.releaseYear,
            braceletTypeEnum: request.braceletTypeEnum,
            description: request.description,
            images: request.images
        );
        var watch = await _watchUseCase.UpdateWatchAsync(command, ct);
        return Ok(watch);
    }
}
