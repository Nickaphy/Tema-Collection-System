using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IIndividualWatchRepository
    {
        Task<Result<IEnumerable<IndividualWatch?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<IndividualWatch>> GetWatchByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<IndividualWatch>> CreateWatchAsync(IndividualWatch watch, CancellationToken ct = default);
        Task<Result> DeleteWatchAsync(Guid id, CancellationToken ct = default);
    }
}
