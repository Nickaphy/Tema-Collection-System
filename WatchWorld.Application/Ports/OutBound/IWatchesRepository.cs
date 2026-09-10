using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound;

public interface IWatchesRepository
{
    Task<Result<IEnumerable<Watches?>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<Watches>> GetWatchByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<Watches>> CreateWatchAsync(Watches watch, CancellationToken ct = default);
}
