using FluentResults;
using Microsoft.EntityFrameworkCore;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters;

public class SqlServerWatchRepository : IWatchesRepository
{
    private readonly AppDbContext _context;

    public SqlServerWatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Watches>>> GetAllWatchesAsync(CancellationToken ct = default)
    {
        var watches = await _context.Watchlist
            .Include(w => w.Images)
            .AsNoTracking()
            .ToListAsync(ct);
        return Result.Ok(watches.AsEnumerable());
    }

    public async Task<Result<Watches>> GetWatchByIdAsync(Guid id, CancellationToken ct = default)
    {
        var watch = await _context.Watchlist.FindAsync(new object[] { id }, ct);
        if (watch == null)
            {
                return Result.Fail("Watch not found.");
            }
        return Result.Ok(watch);
    }
    public async Task<Result<Watches>> CreateWatchAsync(Watches watch, CancellationToken ct = default)
    {
        var result = await _context.Watchlist.AddAsync(watch, ct);
        if (result == null)
            {
                return Result.Fail("Failed to create watch.");
            }
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }

    public async Task<Result<Watches>> UpdateWatchAsync(Watches watch, CancellationToken ct = default)
    {
        _context.Watchlist.Update(watch);
        await _context.SaveChangesAsync(ct);
        return Result.Ok(watch);
    }

    public async Task<Result> DeleteWatchAsync(Guid watchId, CancellationToken ct = default)
    {
        var watch = await _context.Watchlist.FindAsync(new object[] { watchId }, ct);
        if (watch == null)
        {
            return Result.Fail("Watch not found.");
        }
        _context.Watchlist.Remove(watch);
        await _context.SaveChangesAsync(ct);
        return Result.Ok();
    }

    public async Task SaveAsync(Watches watch, CancellationToken ct = default)
    {
        var exists = await _context.Watchlist.AnyAsync(e => e.Id == watch.Id, ct);
        if (!exists)
        {
            await _context.Watchlist.AddAsync(watch, ct);
        }
        else
        {
            _context.Watchlist.Update(watch);
        }

        await _context.SaveChangesAsync(ct);
    }

}