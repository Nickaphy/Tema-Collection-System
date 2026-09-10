using FluentResults;
using WatchWorld.Application.Commands.ListingCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IListingUseCase
    {
        Task<Result<Listing>> CreateListingAsync(CreateListingCommand command, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<Listing?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<Listing>> GetListingByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> DeleteListingAsync(DeleteListingCommand command, CancellationToken cancellationToken = default);
    }
}
