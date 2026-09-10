using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound;

public interface IWatchesRepository
{
    Task<IEnumerable<Watches?>> GetAllAsync(CancellationToken ct = default);
    Task<Watches?> GetWatchByIdAsync(int id, CancellationToken ct = default);
    Task<Watches> CreateWatchAsync(Watches watch, CancellationToken ct = default);
}
