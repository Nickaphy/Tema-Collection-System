namespace WatchWorld.Application.Ports.OutBound.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, bool isAdmin);
    }

    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
