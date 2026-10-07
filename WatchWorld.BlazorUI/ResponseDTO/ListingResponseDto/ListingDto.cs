using WatchWorld.BlazorUI.ResponseDTO.IndividualWatchResponseDto;

namespace WatchWorld.BlazorUI.ResponseDTO.ListingResponseDto
{
    public class ListingDto
    {
        public Guid Id { get; set; }
        public IndividualWatchDto BorrowableWatch { get; set; } = new();
        public decimal PricePerDay { get; set; }
    }

}
