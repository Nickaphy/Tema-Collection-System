namespace WatchWorld.Domain.Entities
{
    public class ActiveBorrows
    {
        public Guid ListingId { get; private set; }
        public Guid BorrowId { get; private set; }

        private ActiveBorrows() { }
        public ActiveBorrows(Guid listingId, Guid borrowId) // Many to many relationship between Listing and Borrow
        {
            ListingId = listingId;
            BorrowId = borrowId;
        }
    }
}
