using FluentResults;
using WatchWorld.Application.Commands.BrandCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IBrandUseCase
    {
        Task<Result<Brand>> CreateBrandAsync(CreateBrandCommand command, CancellationToken ct);
        Task<Result<Brand>> GetBrandByIdAsync(Guid id, CancellationToken ct);
        Task<Result<IEnumerable<Brand>>> GetAllBrandsAsync(CancellationToken ct);
        Task<Result<Brand>> UpdateBrandAsync(UpdateBrandCommand command, CancellationToken ct);
        Task<Result> DeleteBrandAsync(Guid brandId, CancellationToken ct);
    }
}
