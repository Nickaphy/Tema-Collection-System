using WatchWorld.Domain.Service;

namespace WatchWorld.Domain.ValueObjects.ManyToMany
{
    public record WatchBorrow
    {
        public Guid UserId { get; private set; }
        public Guid IndividualWatchId { get; private set; }

        private WatchBorrow() { }
        private WatchBorrow(Guid userId, Guid individualWatchId) // Many to many relationship between User and IndividualWatch
        {
            UserId = userId;
            IndividualWatchId = individualWatchId;
        }

        public static WatchBorrow Create(Guid userId, Guid individualWatchId)
        {
            if (userId == Guid.Empty)
                throw new UserInvalidInputException("UserId cannot be empty.");
            if (individualWatchId == Guid.Empty)
                throw new UserInvalidInputException("IndividualWatchId cannot be empty.");
            return new WatchBorrow(userId, individualWatchId);
        }
    }
}
