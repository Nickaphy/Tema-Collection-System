using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;
using Xunit;

namespace WatchWorld.Test;

public class TimeSlotTests
{
    [Fact]
    public void Overlapping_slots_are_detected()
    {
        var a = new TimeSlot(DateTimeOffset.Now.AddDays(1), DateTimeOffset.Now.AddDays(3));
        var b = new TimeSlot(DateTimeOffset.Now.AddDays(2), DateTimeOffset.Now.AddDays(4));

        Assert.True(a.OverlapWithOtherTimeSlot(b));
    }

    [Fact]
    public void Slots_that_only_touch_do_not_overlap()
    {
        var start = DateTimeOffset.Now.AddDays(1);
        var a = new TimeSlot(start, start.AddDays(1));
        var b = new TimeSlot(start.AddDays(1), start.AddDays(2));

        Assert.False(a.OverlapWithOtherTimeSlot(b));
    }
}