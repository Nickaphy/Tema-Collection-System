using FluentResults;
using Microsoft.EntityFrameworkCore;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters;

public class SqlServerIndividualWatchRepository : IIndividualWatchRepository
{
    private readonly AppDbContext _context;

    public SqlServerIndividualWatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<IndividualWatch>>> GetAllIndividualWatchesAsync(CancellationToken ct = default)
    {
        var watches = await _context.IndividualWatches
            .Include(w => w.SpecificWatch)
            .Include(w => w.Picture)
            .ToListAsync(ct);
        if (watches == null || !watches.Any())
            {
                return Result.Fail<IEnumerable<IndividualWatch>>("No individual watches found.");
            }
        return Result.Ok(watches.AsEnumerable());
    }

    public async Task<Result<IndividualWatch>> GetIndividualWatchByIdAsync(Guid id, CancellationToken ct = default)
    {
        var watch = await _context.IndividualWatches
            .Include(w => w.SpecificWatch)
            .Include(w => w.Picture)
            .FirstOrDefaultAsync(w => w.Id == id, ct);
        if (watch == null)
            {
                return Result.Fail<IndividualWatch>("No individual watch found.");
            }
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }
    

    public async Task<Result<IndividualWatch>> CreateIndividualWatchAsync(IndividualWatch watch, CancellationToken ct = default)
    {
        var result = await _context.IndividualWatches.AddAsync(watch, ct);
        if (result == null)
        {
            return Result.Fail<IndividualWatch>("Failed to create watch.");
        }
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }

    public async Task<Result<IndividualWatch>> UpdateIndividualWatchAsync(IndividualWatch watch, CancellationToken ct = default)
    {
        _context.IndividualWatches.Update(watch);
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }

    public async Task<Result> DeleteIndividualWatchAsync(Guid id, CancellationToken ct = default)
    {
        var watch = await _context.IndividualWatches.FindAsync(new object[] { id }, ct);
        if (watch == null)
        {
            return Result.Fail("Watch not found.");
        }
        _context.IndividualWatches.Remove(watch);
        await _context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

