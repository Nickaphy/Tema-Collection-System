using FluentResults;
using WatchWorld.Application.Commands.BorrowCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IBorrowUseCase
    {
        Task<Result<Borrow>> CreateBorrowAsync(CreateBorrowCommand command, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<Borrow?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<Borrow>> GetBorrowByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> DeleteBorrowAsync(DeleteBorrowCommand command, CancellationToken cancellationToken = default);
        Task<Result<Borrow>> UpdateBorrowTimeSlotAsync(UpdateBorrowTimeSlotCommand command, CancellationToken cancellationToken = default);
        Task<Result<Borrow>> UpdateBorrowStatusAsync(UpdateBorrowStatusCommand command, CancellationToken cancellationToken = default); 
    }
}
