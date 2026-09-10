using FluentResults;
using WatchWorld.Application.Commands.IndividualWatchCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IIndividualWatchUseCase
    {
        Task<Result<IndividualWatch>> CreateIndividualWatchAsync(CreateIndividualWatchCommand command, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<IndividualWatch?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<IndividualWatch>> GetIndividualWatchByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> DeleteIndividualWatchAsync(DeleteIndividualWatchCommand command, CancellationToken cancellationToken = default);
    }
}
