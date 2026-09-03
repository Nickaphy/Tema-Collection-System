using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class UserRating 
    {
        public Guid UserRatingId { get; private set; }
        public Guid RatedToUserId { get; private set; }
        public int RatingAmount { get; private set; }
        public string Description { get; private set; }
        public Guid RatedByUserId { get; private set; }


        private UserRating() { }
    }
}
