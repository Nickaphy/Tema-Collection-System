using FluentResults;
using WatchWorld.Application.Commands.WatchesCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound;

public interface IWatchesUseCase
{
    Task<Result<Watches>> CreateWatchAsync(CreateWatchCommand command, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Watches?>>> GetAllAsync(CancellationToken ct = default);
}
