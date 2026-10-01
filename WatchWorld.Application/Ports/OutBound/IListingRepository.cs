using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IListingRepository
    {
        Task<Result<IEnumerable<Listing>>> GetAllListingsAsync(CancellationToken ct = default);
        Task<Result<Listing>> GetListingByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<Listing>> CreateListingAsync(Listing listing, CancellationToken ct = default);
        Task<Result<Listing>> UpdateListingAsync(Listing listing, CancellationToken ct = default);
        Task<Result> DeleteListingAsync(Guid id, CancellationToken ct = default);
    }
}
