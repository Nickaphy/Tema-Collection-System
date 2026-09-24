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
            var listings = await _listingUseCase.GetAllAsync(ct);
            return Ok(listings);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Listing>> GetById(Guid id, CancellationToken ct)
        {
            var listing = await _listingUseCase.GetListingByIdAsync(id, ct);
            return Ok(listing);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Listing>> Create([FromBody] CreateListingRequest request, CancellationToken ct)
        {
            var command = new CreateListingCommand(
                borrowableWatchId: request.borrowableWatchId,
                pricePerDay: request.pricePerDay
            );

            var listing = await _listingUseCase.CreateListingAsync(command, ct);
            return CreatedAtAction(nameof(Get), new { id = new Guid() }, listing);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Delete(DeleteListingRequest request, CancellationToken ct)
        {
            await _listingUseCase.DeleteListingAsync(new DeleteListingCommand(request.id), ct);
            return NoContent();
        }
    }
}
