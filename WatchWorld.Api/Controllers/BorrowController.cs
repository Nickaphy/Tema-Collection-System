using Microsoft.AspNetCore.Mvc;
using WatchWorld.Api.Requests.BorrowRequests;
using WatchWorld.Api.Requests.ListingRequests;
using WatchWorld.Application.Commands.BorrowCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowUseCase _borrowUseCase;

        public BorrowController(IBorrowUseCase borrowUseCase)
        {
            _borrowUseCase = borrowUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrow>>> Get(CancellationToken ct)
        {
            var borrows = await _borrowUseCase.GetAllAsync(ct);
            return Ok(borrows);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Borrow>> GetById(Guid id, CancellationToken ct)
        {
            var borrow = await _borrowUseCase.GetBorrowByIdAsync(id, ct);
            return Ok(borrow);
        }

        [HttpPost]
        public async Task<ActionResult<Borrow>> Create([FromBody] CreateBorrowRequest request, CancellationToken ct)
        {
            var command = new CreateBorrowCommand(
                borrowedByUserId: request.borrowedByUserId,
                borrowedFromUserId: request.borrowedFromUserId,
                borrowTimeSlot: request.borrowTimeSlot,
                status: request.status
            );
            var borrow = await _borrowUseCase.CreateBorrowAsync(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = new Guid() }, borrow);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(DeleteBorrowRequest request, CancellationToken ct)
        {
            await _borrowUseCase.DeleteBorrowAsync(new DeleteBorrowCommand(request.borrowId), ct);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Borrow>> UpdateTimeSlot(Guid id, [FromBody] UpdateBorrowTimeSlotRequest request, CancellationToken ct)
        {
            var command = new UpdateBorrowTimeSlotCommand(
                id: request.borrowId,
                borrowTimeSlot: request.borrowTimeSlot
            );
            var borrow = await _borrowUseCase.UpdateBorrowTimeSlotAsync(command, ct);
            return Ok(borrow);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<Borrow>> UpdateStatus(Guid id, [FromBody] UpdateBorrowStatusRequest request, CancellationToken ct)
        {
            var command = new UpdateBorrowStatusCommand(
                borrowId: request.borrowId,
                status: request.status,
                targetStatus: request.targetStatus
            );
            var borrow = await _borrowUseCase.UpdateBorrowStatusAsync(command, ct);
            return Ok(borrow);
        }
    }
}
