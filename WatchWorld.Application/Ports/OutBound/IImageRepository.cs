using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IImageRepository
    {
        Task<Result<IEnumerable<HighResImage?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<HighResImage>> GetImageByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<HighResImage>> CreateImageAsync(HighResImage image, CancellationToken ct = default);
        Task<Result> DeleteImageAsync(Guid id, CancellationToken ct = default);
    }
}
