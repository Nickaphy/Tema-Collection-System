using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.Entities
{
    public class UserOwnsWatch
    {
        public Guid UserId { get; private set; }
        public Guid WatchId { get; private set; }


        private UserOwnsWatch() { }
    }
}
