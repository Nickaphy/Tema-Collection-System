using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class User : Aggregateroot
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Note { get; private set; }
        public string Password { get; private set; }
        public bool IsAdmin { get; private set; }
        public List<UserRating> Rating { get; private set; }


        private User() { }
    }
}
