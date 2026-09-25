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
    public class SqlServerUserRatingRepository : IUserRatingRepository
    {
        private readonly AppDbContext _context;

        public SqlServerUserRatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<UserRating>>> GetAllAsync(CancellationToken ct = default)
        {
            var userRatings = await _context.UserRatings.ToListAsync(ct);
            if (userRatings == null || !userRatings.Any())
            {
                return Result.Fail("No user ratings found.");
            }
            return Result.Ok(userRatings.AsEnumerable());
        }

        public async Task<Result<UserRating?>> GetUserRatingByIdAsync(Guid id, CancellationToken ct = default)
        {
            var userRating = await _context.UserRatings.FindAsync(new object[] { id }, ct);
            if (userRating == null)
            {
                return Result.Fail<UserRating?>("User rating not found.");
            }
            return Result.Ok(userRating);
        }

        public async Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var userRatings = await _context.UserRatings
                .Where(ur => ur.Id == userId)
                .ToListAsync(ct);
            if (userRatings == null || !userRatings.Any())
            {
                return Result.Ok(userRatings.Select(r => (UserRating?)r));
            }
            return Result.Ok(userRatings.AsEnumerable());
        }

        public async Task<Result<IEnumerable<UserRating?>>> GetAllUserRatingsToUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var userRatings = await _context.UserRatings
                .Where(ur => ur.Id == userId)
                .ToListAsync(ct);
            if (userRatings == null || !userRatings.Any())
            {
                return Result.Ok(userRatings.Select(r => (UserRating?)r));
            }
            return Result.Ok(userRatings.AsEnumerable());
        }

        public async Task<Result<UserRating>> CreateUserRatingAsync(UserRating userRating, CancellationToken ct = default)
        {
            var result = await _context.UserRatings.AddAsync(userRating, ct);
            if (result == null)
            {
                return Result.Fail("Failed to create user rating.");
            }

            await _context.SaveChangesAsync(ct);
            return Result.Ok(userRating);
        }

        public async Task<Result<UserRating>> UpdateUserRatingAsync(UserRating userRating, CancellationToken ct = default)
        {
            _context.UserRatings.Update(userRating);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(userRating);
        }

        public async Task<Result> DeleteUserRatingAsync(Guid userRatingId, CancellationToken ct = default)
        {
            var userRating = await _context.UserRatings.FindAsync(new object[] { userRatingId }, ct);
            if (userRating == null)
            {
                return Result.Fail("User rating not found.");
            }
            _context.UserRatings.Remove(userRating);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }

    }
}
