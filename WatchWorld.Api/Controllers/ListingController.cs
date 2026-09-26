using Microsoft.AspNetCore.Mvc;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Application.Commands.ListingCommands;
using WatchWorld.Api.Requests.ListingRequests;
using Microsoft.AspNetCore.Authorization;

namespace WatchWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly IListingUseCase _listingUseCase;

        public ListingController(IListingUseCase listingUseCase)
        {
            _listingUseCase = listingUseCase;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Listing>>> Get(CancellationToken ct)
        {
            var result = await _listingUseCase.GetAllAsync(ct);

            if (result.IsFailed)
                return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Listing>> GetById(Guid id, CancellationToken ct)
        {
            var listing = await _listingUseCase.GetListingByIdAsync(id, ct);
            return Ok(listing);
        }

        [HttpPost]
        //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
        public async Task<ActionResult<Listing>> Create([FromBody] CreateListingRequest request, CancellationToken ct)
        {
            var command = new CreateListingCommand(
                borrowableWatchId: request.borrowableWatchId,
                pricePerDay: request.pricePerDay
            );
            var result = await _listingUseCase.CreateListingAsync(command, ct);
            if (result.IsFailed)
                return Problem(string.Join("; ", result.Errors.Select(e => e.Message)));

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
            
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "User,Admin")] // Commented out because Auth hasn't been enabled yet
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _listingUseCase.DeleteListingAsync(new DeleteListingCommand(id), ct);
            return NoContent();
        }
    }
}
