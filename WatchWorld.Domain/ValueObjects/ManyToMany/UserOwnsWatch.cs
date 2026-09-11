using WatchWorld.Domain.Service;

namespace WatchWorld.Domain.ValueObjects.ManyToMany
{
    public record UserOwnsWatch
    {
        public Guid UserId { get; private set; }
        public Guid WatchId { get; private set; }

        private UserOwnsWatch() { }

        private UserOwnsWatch(Guid userId, Guid watchId) // Many to many relationship between User and Watch
        {
            UserId = userId;
            WatchId = watchId;
        }

        public static UserOwnsWatch Create(Guid userId, Guid watchId)
        {
            if (userId == Guid.Empty)
                throw new UserInvalidInputException("UserId cannot be empty.");
            if (watchId == Guid.Empty)
                throw new UserInvalidInputException("WatchId cannot be empty.");
            return new UserOwnsWatch(userId, watchId);
        }
    }
}
