using WatchWorld.Domain.Service;

namespace WatchWorld.Domain.ValueObjects.ManyToMany
{
    public record ActiveBorrows
    {
        public Guid ListingId { get; private set; }
        public Guid BorrowId { get; private set; }

        private ActiveBorrows() { }
        private ActiveBorrows(Guid listingId, Guid borrowId) // Many to many relationship between Listing and Borrow
        {
            ListingId = listingId;
            BorrowId = borrowId;
        }

        public static ActiveBorrows Create(Guid listingId, Guid borrowId)
        {
            if (listingId == Guid.Empty)
                throw new UserInvalidInputException("ListingId cannot be empty.");
            if (borrowId == Guid.Empty)
                throw new UserInvalidInputException("BorrowId cannot be empty.");
            return new ActiveBorrows(listingId, borrowId);
        }
    }
}
