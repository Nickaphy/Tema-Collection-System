namespace WatchWorld.BlazorUI.ResponseDTO.ListingResponseDto
{
    public class CreateListingRequestDto
    {
        public Guid Id { get; set; }
        public Guid BorrowableWatchId { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
