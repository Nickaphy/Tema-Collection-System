using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Infrastructure.Database.Seed;

public static class WatchSeeder
{
    // Idempotent - does nothing if Watchlist already has rows, so it's
    // safe to call on every startup instead of only on a fresh database.
    public static async Task SeedWatchesAsync(AppDbContext context)
    {
        if (await context.Watchlist.AnyAsync())
            return;

        foreach (var seed in WatchSeedData.All)
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
