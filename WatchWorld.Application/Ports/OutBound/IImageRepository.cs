using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IImageRepository
    {
        Task<Result<IEnumerable<HighResImage?>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<HighResImage>> GetImageByIdAsync(int id, CancellationToken ct = default);
        Task<Result<HighResImage>> CreateImageAsync(HighResImage image, CancellationToken ct = default);
        Task<Result<HighResImage>> DeleteImageAsync(Guid id, CancellationToken ct = default);
    }
}
