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

    public async Task<Result<IEnumerable<Watches>>> GetAllAsync(CancellationToken ct = default)
    {
        var watches = await _context.Watchlist
            .Include(w => w.Images)
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<Watches>>(watches);
    }

    public async Task<Result<Watches?>> GetWatchByIdAsync(Guid id, CancellationToken ct = default)
    {
        var watch = await _context.Watchlist.FindAsync(new object[] { id }, ct);
        return Result.Ok(watch);
    }
    public async Task<Result<Watches>> CreateWatchAsync(Watches watch, CancellationToken ct = default)
    {
        await _context.Watchlist.AddAsync(watch, ct);
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
            return Result.Fail($"Uret kunne ikke findes.");
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