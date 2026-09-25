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
    public class SqlServerBorrowRepository : IBorrowRepository
    {
        private readonly AppDbContext _context;

        public SqlServerBorrowRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Borrow>>> GetAllAsync(CancellationToken ct = default)
        {
            var borrows = await _context.Borrows.ToListAsync(ct);
            if (borrows == null || !borrows.Any())
            {
                return Result.Fail<IEnumerable<Borrow>>("No borrows found.");
            }
            return Result.Ok(borrows.AsEnumerable());
        }

        public async Task<Result<Borrow>> GetBorrowByIdAsync(Guid id, CancellationToken ct = default)
        {
            var borrow = await _context.Borrows.FindAsync(new object[] { id }, ct);
            if (borrow == null)
            {
                return Result.Fail<Borrow>("Borrow not found.");
            }
            return Result.Ok(borrow);
        }

        public async Task<Result<IEnumerable<Borrow>>> GetBorrowsByUserIdAsync(Guid id, CancellationToken ct = default)
        {
            var borrows = _context.Borrows.Where(b => b.BorrowedByUserId == id).ToAsyncEnumerable();
            var borrowsList = await _context.Borrows
                .Where(b => b.BorrowedByUserId == id)
                .ToListAsync(ct);

            if (borrowsList == null || !borrowsList.Any())
            {
                return Result.Fail("No borrows found for the specified user.");
            }

            return Result.Ok<IEnumerable<Borrow>>(borrowsList);
        }

        public async Task<Result<Borrow>> CreateBorrowAsync(Borrow borrow, CancellationToken ct = default)
        {
            var result = await _context.Borrows.AddAsync(borrow, ct);
            if (result == null)
            {
                return Result.Fail<Borrow>("Failed to create borrow.");
            }

            await _context.SaveChangesAsync(ct);
            return Result.Ok(borrow);
        }

        public async Task<Result<Borrow>> UpdateBorrowTimeSlotAsync(Borrow borrow, CancellationToken ct = default)
        {
            _context.Borrows.Update(borrow);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(borrow);
        }

        public async Task<Result<Borrow>> UpdateBorrowStatusAsync(Borrow borrow, CancellationToken ct = default)
        {
            _context.Borrows.Update(borrow);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(borrow);
        }

        public async Task<Result> DeleteBorrowAsync(Guid borrowId, CancellationToken ct = default)
        {
            var borrow = await _context.Borrows.FindAsync(new object[] { borrowId }, ct);
            if (borrow == null)
            {
                return Result.Fail("Borrow not found.");
            }
            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }

    }
}
