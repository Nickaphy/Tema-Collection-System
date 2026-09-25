using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters
{
    public class SqlServerListingRepository : IListingRepository
    {
        private readonly AppDbContext _context;

        public SqlServerListingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Listing>>> GetAllAsync(CancellationToken ct = default)
        {
            var listings = await _context.Listings
                .Include(l => l.BorrowableWatch)
                    .ThenInclude(iw => iw.SpecificWatch)
                        .ThenInclude(w => w.Images)
                .Include(l => l.BorrowableWatch)
                    .ThenInclude(iw => iw.Picture)
                .AsNoTracking()
                .ToListAsync(ct);

            return Result.Ok<IEnumerable<Listing>>(listings);
        }

        public async Task<Result<Listing>> GetListingByIdAsync(Guid id, CancellationToken ct = default)
        {
            var listing = await _context.Listings.FindAsync(new object[] { id }, ct);
            if (listing == null)
            {
                return Result.Fail<Listing>("Listing not found.");
            }
            return Result.Ok(listing);
        }

        public async Task<Result<Listing>> CreateListingAsync(Listing listing, CancellationToken ct = default)
        {
            var result = await _context.Listings.AddAsync(listing, ct);
            if (result == null)
            {
                return Result.Fail<Listing>("Failed to create listing.");
            }

            await _context.SaveChangesAsync(ct);
            return Result.Ok(listing);
        }

        public async Task<Result<Listing>> UpdateListingAsync(Listing listing, CancellationToken ct = default)
        {
            _context.Listings.Update(listing);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(listing);
        }

        public async Task<Result> DeleteListingAsync(Guid listingId, CancellationToken ct = default)
        {
            var listing = await _context.Listings.FindAsync(new object[] { listingId }, ct);
            if (listing == null)
            {
                return Result.Fail("Listing not found.");
            }
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }

    }
}
