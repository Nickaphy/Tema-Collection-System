namespace WatchWorld.BlazorUI.ResponseDTO
{
    public class WatchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public int CaseSize { get; set; }
        public string? Description { get; set; }
        public DateOnly ReleaseYear { get; set; }
        public List<HighResImageDto> Images { get; set; } = new();
    }

}
