using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Infrastructure.Database.Seed;

public static class WatchSeeder
{
    // Idempotent - does nothing if Watchlist already has rows, so it's
    // safe to call on every startup instead of only on a fresh database.
    // useFullCatalog picks all 150 seeded watches (the shared Mother DB)
    // vs. the smaller Basic slice (everyone's own Local dev DB) - see
    // DatabaseSetup's SeedFullCatalog config flag for where this comes from.
    public static async Task SeedWatchesAsync(AppDbContext context, bool useFullCatalog)
    {
        if (await context.Watchlist.AnyAsync())
            return;

        var seedSet = useFullCatalog ? WatchSeedData.All : WatchSeedData.Basic;

        foreach (var seed in seedSet)
        {
            var watch = Watches.Create(
                seed.Name,
                seed.ModelNumber,
                seed.CaseSize,
                seed.CaseShape,
                seed.CaseMaterial,
                seed.Movement,
                seed.Style,
                seed.OriginalPrice,
                seed.Gender,
                new DateOnly(seed.ReleaseYear, 1, 1),
                new List<BraceletTypeEnum> { seed.Bracelet },
                seed.Description,
                new List<HighResImage>());

            context.Watchlist.Add(watch);
        }

        await context.SaveChangesAsync();
    }
}
