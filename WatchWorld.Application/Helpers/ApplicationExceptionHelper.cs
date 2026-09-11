namespace WatchWorld.Domain.Service
{
    public class UseCaseException(string message) : Exception(message);

    public sealed class UserNotFoundException(string message) : UseCaseException(message);

    public sealed class WatchNotFoundException(string message) : UseCaseException(message);

}

