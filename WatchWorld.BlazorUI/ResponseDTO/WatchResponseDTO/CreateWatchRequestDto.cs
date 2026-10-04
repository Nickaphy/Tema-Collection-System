namespace WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO
{
    public class CreateWatchRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public string ModelNumber { get; set; } = string.Empty;
        public int CaseSize { get; set; }
        public int CaseShapeEnum { get; set; }
        public int CaseMaterialEnum { get; set; }
        public int MovementTypeEnum { get; set; }
        public string Style { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public int GenderEnum { get; set; }
        public DateOnly ReleaseYear { get; set; }
        public List<int> BraceletTypeEnum { get; set; } = new();
        public string? Description { get; set; }
        public List<CreateImageRequestDto> Images { get; set; } = new();
    }

}
