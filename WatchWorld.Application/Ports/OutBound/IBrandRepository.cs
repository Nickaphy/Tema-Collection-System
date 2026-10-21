using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IBrandRepository
    {
        Task<Result<Brand>> CreateBrandAsync(Brand brand, CancellationToken ct);
        Task<Result<Brand>> GetBrandByIdAsync(Guid id, CancellationToken ct);
        Task<Result<IEnumerable<Brand>>> GetAllBrandsAsync(CancellationToken ct);
        Task<Result<Brand>> UpdateBrandAsync(Brand brand, CancellationToken ct);
        Task<Result> DeleteBrandAsync(Guid brandId, CancellationToken ct);
    }
}
