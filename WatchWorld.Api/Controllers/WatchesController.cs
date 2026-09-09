using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.WatchRequests;
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
    public async Task<ActionResult<IEnumerable<Watches>>> Get(CancellationToken ct)
    {
        var watches = await _watchUseCase.GetAllAsync(ct);
        return Ok(watches);
    }

    [HttpPost]
    public async Task<ActionResult<Watches>> Create([FromBody] CreateWatchRequest request, CancellationToken ct)
    {
        var watch = await _watchUseCase.CreateWatchAsync(request.name, request.modelNumber, request.caseSize, request.caseShapeEnum, request.caseMaterialEnum, request.movementTypeEnum, request.style, request.originalPrice, request.genderEnum, request.releaseYear, request.braceletTypeEnum, request.description, request.images, ct);
        return CreatedAtAction(nameof(Get), new { id = watch.Id }, watch);
    }
}
