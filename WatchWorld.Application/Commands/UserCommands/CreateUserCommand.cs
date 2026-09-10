using WatchWorld.Domain.Entities;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Application.Commands.UserCommands
{
    public record CreateUserCommand(
        string firstName,
        string lastName,
        string phoneNumber,
        string email,
        string address,
        string city,
        string? note,
        string password,
        bool isAdmin,
        List<UserRating> rating
    )
    {
    }
}
