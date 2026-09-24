namespace WatchWorld.BlazorUI.ResponseDTO
{
    public class IndividualWatchDto
    {
        public Guid Id { get; set; }
        public WatchDto SpecificWatch { get; set; } = new();
        public int Age { get; set; }
        public List<HighResImageDto> Picture { get; set; } = new();
    }

}
