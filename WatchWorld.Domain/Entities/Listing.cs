using System.Runtime.InteropServices;
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class Listing : Aggregateroot
    {
        public IndividualWatch BorrowableWatch { get; private set; }
        public decimal PricePerDay { get; private set; }


        private Listing() { }

        private Listing(IndividualWatch borrowableWatch, decimal pricePerDay)
        {
            BorrowableWatch = borrowableWatch;
            PricePerDay = pricePerDay;
        }

        public static Listing Create(IndividualWatch borrowableWatch, decimal pricePerDay)
        {
            // Validate the input parameters
            if (borrowableWatch == null)
                throw new UserInvalidInputException("Der skal vælges et ur");
            if (pricePerDay < 0)
                throw new UserInvalidInputException("En pris må ikke være negativ");
            var listing = new Listing(borrowableWatch, pricePerDay);
            return listing;
        }
    }
}
