namespace WatchWorld.BlazorUI.ResponseDTO.UserRatingDTO
{
    public class UserRatingDto
    {
        public Guid Id { get; set; }
        public Guid RatedTargetId { get; set; }
        public Guid RatedByUserId { get; set; }
        public int RatingAmount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool? IsRatingWatch { get; set; }
    }

}
