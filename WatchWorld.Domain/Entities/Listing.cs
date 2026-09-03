using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class Listing : Aggregateroot
    {
        public IndividualWatch BorrowableWatch { get; private set; }
        public decimal PricePerDay { get; private set; }


        private Listing() { }
    }
}
