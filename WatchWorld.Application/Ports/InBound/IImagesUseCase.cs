using FluentResults;
using WatchWorld.Application.Commands.ImageCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IImagesUseCase
    {
        Task<Result<HighResImage>> CreateImageAsync(CreateImageCommand command, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<HighResImage?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result> DeleteImageAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
