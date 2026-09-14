using WatchWorld.Domain.Entities;

namespace WatchWorld.Application.Commands.UserCommands
{
    public record UpdateUserCommand(
        Guid id,
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
