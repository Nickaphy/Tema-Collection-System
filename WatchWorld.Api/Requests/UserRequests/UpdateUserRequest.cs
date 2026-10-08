using WatchWorld.Domain.Entities;

namespace WatchWorld.Api.Requests.UserRequests
{
    public record UpdateUserRequest(
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
