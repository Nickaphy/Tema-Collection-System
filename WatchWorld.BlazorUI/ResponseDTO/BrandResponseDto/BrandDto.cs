namespace WatchWorld.BlazorUI.ResponseDTO.BrandResponseDto
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int FoundingYear { get; set; }
        public string CountryOfOrigin { get; set; } = string.Empty;
        public string? ParentGroup { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string FounderName { get; set; } = string.Empty;
        public string FounderDescriptionNote { get; set; } = string.Empty;
        public string? OriginDescriptionNote { get; set; }
        public string BrandDescriptionNote { get; set; } = string.Empty;
        public string WatchStyleDescriptionNote { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

}
