namespace WatchWorld.BlazorUI.ResponseDTO.UserRatingResponseDTO
{
    public class CreateUserRatingRequestDto
    {
        public Guid RatedToUserId { get; set; }
        public Guid RatedByUserId { get; set; }
        public int RatingAmount { get; set; }
        public bool? IsRatingWatch { get; set; }
        public string Description { get; set; } = string.Empty;
    }

}
