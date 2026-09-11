using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IBorrowRepository
    {
        Task<Result<Borrow>> CreateBorrowAsync(Borrow borrow, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<Borrow?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<Borrow>> GetBorrowByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> DeleteBorrowAsync(Borrow borrow, CancellationToken cancellationToken = default);
        Task<Result<Borrow>> UpdateBorrowAsync(Borrow borrow, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<Borrow>>> GetBorrowsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result<Borrow>> UpdateBorrowStatusAsync(Borrow borrow, CancellationToken cancellationToken = default);
    }
}
