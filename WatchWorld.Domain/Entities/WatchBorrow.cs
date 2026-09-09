namespace WatchWorld.Domain.Entities
{
     public class WatchBorrow
    {
        public Guid UserId { get; private set; }
        public Guid IndividualWatchId { get; private set; }

        private WatchBorrow() { }
        public WatchBorrow(Guid userId, Guid individualWatchId) // Many to many relationship between User and IndividualWatch
        {
            UserId = userId;
            IndividualWatchId = individualWatchId;
        }
    }
}
