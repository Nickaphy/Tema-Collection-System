using FluentResults;
using WatchWorld.Application.Commands.BrandCommands;
using WatchWorld.Application.Ports.InBound;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
namespace WatchWorld.Application.Services;

public class BrandService : IBrandUseCase
{
    private readonly IBrandRepository _brandRepository;
    private static readonly SemaphoreSlim _Lock = new(1, 1);
    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<Result<IEnumerable<Brand>>> GetAllBrandsAsync(CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var brands = await _brandRepository.GetAllBrandsAsync(ct);
            return Result.Ok(brands.Value);
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result<Brand>> CreateBrandAsync(CreateBrandCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var brand = Brand.Create(
                name: command.name,
                foundingYear: command.foundingYear,
                countryOfOrigin: command.countryOfOrigin,
                parentGroup: command.parentGroup,
                logoUrl: command.logoUrl,
                websiteUrl: command.websiteUrl,
                founderName: command.founderName,
                founderDescriptionNote: command.founderDescriptionNote,
                originDescriptionNote: command.originDescriptionNote,
                brandDescriptionNote: command.brandDescriptionNote,
                watchStyleDescriptionNote: command.watchStyleDescriptionNote,
                isActive: command.isActive
            );
            await _brandRepository.CreateBrandAsync(brand, ct);
            return Result.Ok(brand);
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result<Brand>> GetBrandByIdAsync(Guid brandId, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var brand = await _brandRepository.GetBrandByIdAsync(brandId, ct);
            if (brand.IsFailed)
            {
                return Result.Fail("Brand not found.");
            }
            return Result.Ok(brand.Value);
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result<Brand>> UpdateBrandAsync(UpdateBrandCommand command, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var brand = await _brandRepository.GetBrandByIdAsync(command.id, ct);
            if (brand.IsFailed)
            {
                return Result.Fail("Brand not found.");
            }
            var updatedBrand = Brand.Update(
                existingBrand: brand.Value,
                parentGroup: command.parentGroup,
                logoUrl: command.logoUrl,
                websiteUrl: command.websiteUrl,
                founderDescriptionNote: command.founderDescriptionNote,
                originDescriptionNote: command.originDescriptionNote,
                brandDescriptionNote: command.brandDescriptionNote,
                watchStyleDescriptionNote: command.watchStyleDescriptionNote,
                isActive: command.isActive
            );
            await _brandRepository.UpdateBrandAsync(updatedBrand, ct);
            return Result.Ok(updatedBrand);
        }
        finally
        {
            _Lock.Release();
        }
    }
    public async Task<Result> DeleteBrandAsync(Guid brandId, CancellationToken ct = default)
    {
        await _Lock.WaitAsync();
        try
        {
            var brand = await _brandRepository.GetBrandByIdAsync(brandId, ct);
            if (brand.IsFailed)
            {
                return Result.Fail("Brand not found.");
            }
            await _brandRepository.DeleteBrandAsync(brand.Value.Id, ct);
            return Result.Ok();
        }
        finally
        {
            _Lock.Release();
        }
    }
}
