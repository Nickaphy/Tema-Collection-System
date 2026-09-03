using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.Entities
{
    public class Listing
    {
        public Guid ListningId { get; private set; }
        public IndividualWatch BorrowableWatch { get; private set; }
        public decimal PricePerDay { get; private set; }


        private Listing() { }
    }
}
