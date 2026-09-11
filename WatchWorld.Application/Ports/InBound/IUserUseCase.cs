using FluentResults;
using WatchWorld.Application.Commands.UserCommands;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Ports.InBound
{
    public interface IUserUseCase
    {
        Task<Result<User>> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken = default);
        Task<Result<User>> UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserAsync(DeleteUserCommand command, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<User?>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    }
}
