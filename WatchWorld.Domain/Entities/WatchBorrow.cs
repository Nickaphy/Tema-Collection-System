using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.Entities
{
     public class WatchBorrow
    {
        public Guid UserId { get; private set; }
        public Guid IndividualWatchId { get; private set; }
    }
}
