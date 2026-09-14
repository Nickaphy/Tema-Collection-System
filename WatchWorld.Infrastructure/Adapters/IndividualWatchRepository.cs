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

    public async Task<Result<IEnumerable<IndividualWatch>>> GetAllAsync(CancellationToken ct = default)
    {
        var watches = await _context.IndividualWatches
            .Include(w => w.SpecificWatch)
            .Include(w => w.Picture)
            .ToListAsync(ct);
        return Result.Ok(watches.AsEnumerable());
    }

    public async Task<Result<IndividualWatch?>> GetWatchByIdAsync(Guid id, CancellationToken ct = default)
    {
        var watch = await _context.IndividualWatches
            .Include(w => w.SpecificWatch)
            .Include(w => w.Picture)
            .FirstOrDefaultAsync(w => w.Id == id, ct);

        return Result.Ok(watch);
    }
    

    public async Task<Result<IndividualWatch>> CreateWatchAsync(IndividualWatch watch, CancellationToken ct = default)
    {
        await _context.IndividualWatches.AddAsync(watch, ct);
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }

    public async Task<Result<IndividualWatch>> UpdateWatchAsync(IndividualWatch watch, CancellationToken ct = default)
    {
        _context.IndividualWatches.Update(watch);
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }

    public async Task<Result> DeleteWatchAsync(Guid id, CancellationToken ct = default)
    {
        var watch = await _context.IndividualWatches.FindAsync(new object[] { id }, ct);
        if (watch == null)
        {
            return Result.Fail($"Uret kunne ikke findes.");
        }
        _context.IndividualWatches.Remove(watch);
        await _context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}

