using System.Text.Json;
using WatchWorld.BlazorUI.ResponseDTO.WatchResponseDTO;

namespace WatchWorld.BlazorUI.ResponseDTO.IndividualWatchResponseDto
{
    public class IndividualWatchDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SpecificWatchId { get; set; }
        public WatchDto SpecificWatch { get; set; } = new();
        public JsonElement WearGrade { get; set; }
        public int Age { get; set; }
        public string? Note { get; set; }
        public decimal EstimatedValue { get; set; }
        public List<HighResImageDto> Picture { get; set; } = new();
    }
}
