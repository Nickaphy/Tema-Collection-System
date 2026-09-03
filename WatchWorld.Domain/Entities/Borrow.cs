using System.Runtime.InteropServices;
using WatchWorld.Domain.Enums;
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class Borrow : Aggregateroot
    {
        public Guid BorrowedByUserId { get; private set; }
        public Guid BorrowedFromUserId { get; private set; }
        public TimeSlot BorrowTimeSlot { get; private set; }
        public BorrowStatus Status { get; private set; }
        public bool IsActive => Status == BorrowStatus.Active;
        public TimeSpan BorrowTime => BorrowTimeSlot.To - BorrowTimeSlot.From;

        private Borrow() { }

        private Borrow(Guid borrowedByUserId, Guid borrowedFromUserId, TimeSlot borrowTimeSlot, BorrowStatus status)
        {
            BorrowedByUserId = borrowedByUserId;
            BorrowedFromUserId = borrowedFromUserId;
            BorrowTimeSlot = borrowTimeSlot;
            Status = status;
        }


        public static Borrow Create(Guid borrowerId, Guid lenderId, TimeSlot borrowTimeSlot, IEnumerable<Borrow> existingBorrows, BorrowStatus status = BorrowStatus.Active)
        {
            Borrow borrow = new Borrow(borrowerId, lenderId, borrowTimeSlot, status);
            ValidateOverlap(existingBorrows, borrowTimeSlot);
            return borrow;
        }

        public void CompleteBorrow()
        {
            if (Status != BorrowStatus.Active)
                throw new UserInvalidInputException("Kan ikke afslutte en ikke aktiv borrow");
            Status = BorrowStatus.Completed;
        }

        public void CancelBorrow()
        {
            if (Status != BorrowStatus.Active)
                throw new UserInvalidInputException("Kan ikke annullere en ikke aktiv borrow");
            Status = BorrowStatus.Cancelled;
        }

        private static void ValidateOverlap(
            IEnumerable<Borrow> existingWatchBorrows,
            TimeSlot borrowTimeSlot,
            Guid? currentBorrowId = null
            ) //Validation method to check if the watch is already borrowed
        {
            var borrowOverlap = existingWatchBorrows
            .Where(c => c.Id != currentBorrowId)
            .Where(c => c.IsActive)
            .FirstOrDefault(c =>
                borrowTimeSlot.OverlapWithOtherTimeSlot(c.BorrowTimeSlot)
                && c.BorrowTimeSlot.From != borrowTimeSlot.To
                && c.BorrowTimeSlot.To != borrowTimeSlot.From
            );

            if (borrowOverlap is not null)
            {
                throw new ValidationException(
                    $"Uret har allerede en booking"
                    + $"({borrowTimeSlot.From:HH:mm}-{borrowTimeSlot.To:HH:mm}) "
                    + $"der overlapper med nuværende booking");
            }

        }
    }
}
