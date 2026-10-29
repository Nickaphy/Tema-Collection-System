using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Responses
{
    // What the API shows about a user. Never includes the password.
    public record UserResponse(
        Guid id,
        string firstName,
        string lastName,
        string phoneNumber,
        string email,
        string address,
        string city,
        string? note,
        bool isAdmin
    )
    {
        // Turns a User entity into a UserResponse
        public static UserResponse FromUser(User user) => new(
            id: user.Id,
            firstName: user.FirstName,
            lastName: user.LastName,
            phoneNumber: user.PhoneNumber,
            email: user.Email,
            address: user.Address,
            city: user.City,
            note: user.Note,
            isAdmin: user.IsAdmin
        );
    }
}