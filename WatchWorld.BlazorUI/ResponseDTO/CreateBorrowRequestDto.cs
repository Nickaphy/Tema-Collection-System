namespace WatchWorld.BlazorUI.ResponseDTO
{
    public class CreateBorrowRequestDto
    {
        public Guid BorrowedByUserId { get; set; }
        public Guid BorrowedFromUserId { get; set; }
        public TimeSlotDto BorrowTimeSlot { get; set; } = new();
        public int Status { get; set; } // Defaults to BorrowStatus.Active = 0
    }

}
