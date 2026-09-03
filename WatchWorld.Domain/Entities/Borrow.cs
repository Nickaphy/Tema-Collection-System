using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.Entities
{
    public class Borrow 
    {
        public Guid BorrowId { get; private set; }
        public User BorrowedByUser { get; private set; }
        public User BorrowedFromUser { get; private set; }
        public bool IsActive { get; private set; }
        public DateOnly BorrowedFrom { get; private set; }
        public DateOnly BorrowedTo { get; private set; }
        public TimeSpan BorrowTime => BorrowedTo.ToDateTime(TimeOnly.MinValue) - BorrowedFrom.ToDateTime(TimeOnly.MinValue);


        private Borrow() { }
    }
}
