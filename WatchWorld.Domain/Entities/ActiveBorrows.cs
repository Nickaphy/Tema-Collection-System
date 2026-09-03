using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.Entities
{
    public class ActiveBorrows
    {
        public Guid ListingId { get; private set; }
        public Guid BorrowId { get; private set; }
    }
}
