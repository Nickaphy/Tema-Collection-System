namespace WatchWorld.BlazorUI.ResponseDTO
{
    public class ListingDto
    {
        public Guid Id { get; set; }
        public IndividualWatchDto BorrowableWatch { get; set; } = new();
        public decimal PricePerDay { get; set; }
    }

}
