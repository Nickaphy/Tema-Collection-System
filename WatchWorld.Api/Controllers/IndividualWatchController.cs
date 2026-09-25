using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.IndividualWatchRequests;
using WatchWorld.Application.Commands.IndividualWatchCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndividualWatchController : ControllerBase
    {
        private readonly IIndividualWatchUseCase _individualWatchUseCase;

        public IndividualWatchController(IIndividualWatchUseCase individualWatchUseCase)
        {
            _individualWatchUseCase = individualWatchUseCase;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<IndividualWatch>>> Get(CancellationToken ct)
        {
            var individualWatches = await _individualWatchUseCase.GetAllAsync(ct);
            if (individualWatches.IsFailed)
                return Problem(string.Join("; ", individualWatches.Errors.Select(e => e.Message)));
            return Ok(individualWatches.Value);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IndividualWatch>> GetById(Guid id, CancellationToken ct)
        {
            var individualWatch = await _individualWatchUseCase.GetIndividualWatchByIdAsync(id, ct);
            return Ok(individualWatch);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IndividualWatch>> Create([FromBody] CreateIndividualWatchRequest request, CancellationToken ct)
        {
            var command = new CreateIndividualWatchCommand(
                specificWatchId: request.specificWatchId,
                wearGrade: request.wearGrade,
                age: request.age,
                note: request.note,
                estimatedValue: request.estimatedValue,
                picture: request.picture
            );

            var individualWatch = await _individualWatchUseCase.CreateIndividualWatchAsync(command, ct);
            return CreatedAtAction(nameof(Get), new { id = new Guid() }, individualWatch);
        }

        public async Task<ActionResult<IndividualWatch>> UpdateWatch([FromBody] UpdateIndividualWatchRequest request, CancellationToken ct)
        {
            var command = new UpdateIndividualWatchCommand(
                individualWatchId: request.individualWatchId,
                specificWatchId: request.specificWatchId,
                wearGrade: request.wearGrade,
                age: request.age,
                note: request.note,
                estimatedValue: request.estimatedValue,
                picture: request.picture
            );
            var individualWatch = await _individualWatchUseCase.UpdateIndividualWatchAsync(command, ct);
            return Ok(individualWatch);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Delete(DeleteIndividualWatchRequest request, CancellationToken ct)
        {
            await _individualWatchUseCase.DeleteIndividualWatchAsync(new DeleteIndividualWatchCommand(request.id), ct);
            return NoContent();
        }
    }
}