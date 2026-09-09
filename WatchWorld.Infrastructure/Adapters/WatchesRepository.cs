using Microsoft.EntityFrameworkCore;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters;

public class SqliteWatchRepository : IWatchesRepository
{
    private readonly AppDbContext _context;

    public SqliteWatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Watches>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Watchlist.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Watches?> GetWatchByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Watchlist.FindAsync(new object[] { id }, ct);
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