using System.Text.Json;

namespace WatchWorld.BlazorUI.ResponseDTO.IndividualWatchResponseDto
{
    public class CreateIndividualWatchRequestDto
    {
        public Guid UserId { get; set; }
        public Guid SpecificWatchId { get; set; }
        public JsonElement WearGrade { get; set; }
        public int Age { get; set; }
        public string? Note { get; set; }
        public decimal EstimatedValue { get; set; }
        public List<CreateImageRequestDto> Picture { get; set; } = new();

    }
}
