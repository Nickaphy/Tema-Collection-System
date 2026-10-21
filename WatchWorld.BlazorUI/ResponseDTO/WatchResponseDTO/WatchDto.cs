using System.Text.Json;
using WatchWorld.Domain.Entities;

namespace WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO
{
    public class WatchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public string ModelNumber { get; set; } = string.Empty;
        public int CaseSize { get; set; }
        public JsonElement CaseShapeEnum { get; set; }
        public JsonElement CaseMaterialEnum { get; set; }
        public JsonElement MovementTypeEnum { get; set; }
        public string Style { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public JsonElement GenderEnum { get; set; }
        public DateOnly ReleaseYear { get; set; }
        public List<JsonElement> BraceletTypeEnum { get; set; } = new();
        public string? Description { get; set; }
        public List<HighResImageDto> Images { get; set; } = new();
    }
}
