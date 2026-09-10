using FluentResults;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.OutBound
{
    public interface IUserRepository
    {
        Task<Result<IEnumerable<User?>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<Result<User?>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result<User>> CreateUserAsync(User user, CancellationToken ct = default);
        Task<Result<User>> UpdateUserAsync(User user, CancellationToken ct = default);
        Task<Result<User>> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
