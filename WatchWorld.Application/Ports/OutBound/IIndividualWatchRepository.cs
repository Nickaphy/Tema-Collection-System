using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IIndividualWatchRepository
    {
        Task<Result<IEnumerable<IndividualWatch>>> GetAllIndividualWatchesAsync(CancellationToken ct = default);
        Task<Result<IndividualWatch>> GetIndividualWatchByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<IEnumerable<IndividualWatch>>> GetIndividualWatchesByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<Result<IndividualWatch>> UpdateIndividualWatchAsync(IndividualWatch watch, CancellationToken ct = default);
        Task<Result<IndividualWatch>> CreateIndividualWatchAsync(IndividualWatch watch, CancellationToken ct = default);
        Task<Result> DeleteIndividualWatchAsync(Guid id, CancellationToken ct = default);
    }
}
