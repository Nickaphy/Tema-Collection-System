using WatchWorld.Domain.Service;

namespace WatchWorld.Domain.ValueObjects
{
    public class TimeSlot
    {
        public TimeSlot() { }
        public TimeSlot(DateTimeOffset from, DateTimeOffset to)
        {
            if (to <= from)
                throw new DomainException("Til tiden må ikke være før fra tiden");
            else if (from < DateTime.Now)
                throw new DomainException("Fra tiden må ikke være i fortiden");
            else
            {
                From = from;
                To = to;
            } 
        }

        public DateTimeOffset From { get; }
        public DateTimeOffset To { get; }

        public bool OverlapWithOtherTimeSlot(TimeSlot other)
        {
            return From < other.To && other.From < To;
        }
    }
}
