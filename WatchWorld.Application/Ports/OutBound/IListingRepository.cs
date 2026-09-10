using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IListingRepository
    {
        Task<Result<IEnumerable<Listing?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<Listing>> GetListingByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<Listing>> CreateListingAsync(Listing listing, CancellationToken ct = default);
        Task<Result> DeleteListingAsync(Guid id, CancellationToken ct = default);
    }
}
