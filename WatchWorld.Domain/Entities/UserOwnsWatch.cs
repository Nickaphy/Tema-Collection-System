namespace WatchWorld.Domain.Entities
{
    public class UserOwnsWatch
    {
        public Guid UserId { get; private set; }
        public Guid WatchId { get; private set; }

        private UserOwnsWatch() { }

        public UserOwnsWatch(Guid userId, Guid watchId) // Many to many relationship between User and Watch
        {
            UserId = userId;
            WatchId = watchId;
        }
    }
}
