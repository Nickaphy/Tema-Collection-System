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

    public async Task<IndividualWatch?> GetIndividualWatchByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.IndividualWatches
            .Include(w => w.SpecificWatch)
            .Include(w => w.Picture)
            .FirstOrDefaultAsync(w => w.Id == id, ct);
    }

    public async Task<IndividualWatch> CreateIndividualWatchAsync(IndividualWatch watch, CancellationToken ct = default)
    {
        await _context.IndividualWatches.AddAsync(watch, ct);
        await _context.SaveChangesAsync(ct);
        return watch;
    }

    public async Task<IndividualWatch> UpdateIndividualWatchAsync(IndividualWatch watch, CancellationToken ct = default)
    {
        _context.IndividualWatches.Update(watch);
        await _context.SaveChangesAsync(ct);
        return watch;
    }
}

