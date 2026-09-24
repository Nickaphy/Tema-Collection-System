using FluentResults;
using Microsoft.EntityFrameworkCore;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters
{
    public class SqlServerUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public SqlServerUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<User>>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _context.Users.ToListAsync(ct);
            if (users == null || !users.Any())
            {
                return Result.Fail<IEnumerable<User>>("No users found.");
            }
            return Result.Ok(users.AsEnumerable());
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _context.Users.FindAsync(new object[] { id }, ct);
            if (user == null)
            {
                return Result.Fail<User>("User not found.");
            }
            return Result.Ok(user);
        }

        public async Task<Result<User>> CreateUserAsync(User user, CancellationToken ct = default)
        {
            var result = await _context.Users.AddAsync(user, ct);
            if (result == null)
            {
                return Result.Fail<User>("Failed to create user.");
            }

            await _context.SaveChangesAsync(ct);
            return Result.Ok(user);
        }

        public async Task<Result<User>> UpdateUserAsync(User user, CancellationToken ct = default)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(user);
        }

        public async Task<Result> DeleteUserAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _context.Users.FindAsync(new object[] { userId }, ct);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }

        public async Task<Result> SetUserAsAdminAsync(Guid userId, bool shouldBeAdmin, CancellationToken ct = default)
        {
            var user = await _context.Users.FindAsync(new object[] { userId }, ct);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }

    }
}
