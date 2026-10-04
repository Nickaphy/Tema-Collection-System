using FluentResults;
using Microsoft.EntityFrameworkCore;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters
{
    public class SqlServerBrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;

        public SqlServerBrandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Brand>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await _context.Brands.ToListAsync(ct);
            return Result.Ok(brands.AsEnumerable());
        }

        public async Task<Result<Brand>> GetBrandByIdAsync(Guid id, CancellationToken ct = default)
        {
            var brand = await _context.Brands.FindAsync(new object[] { id }, ct);
            if (brand == null)
            {
                return Result.Fail("Brand not found.");
            }
            return Result.Ok(brand);
        }

        public async Task<Result<Brand>> CreateBrandAsync(Brand brand, CancellationToken ct = default)
        {
            var result = await _context.Brands.AddAsync(brand, ct);
            if (result == null)
            {
                return Result.Fail("Failed to create brand.");
            }

            await _context.SaveChangesAsync(ct);
            return Result.Ok(brand);
        }

        public async Task<Result<Brand>> UpdateBrandAsync(Brand brand, CancellationToken ct = default)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(brand);
        }

        public async Task<Result> DeleteBrandAsync(Guid brandId, CancellationToken ct = default)
        {
            var brand = await _context.Brands.FindAsync(new object[] { brandId }, ct);
            if (brand == null)
            {
                return Result.Fail("Brand not found.");
            }
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
