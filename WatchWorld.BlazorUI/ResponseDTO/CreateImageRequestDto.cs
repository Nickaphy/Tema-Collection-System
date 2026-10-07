namespace WatchWorld.BlazorUI.ResponseDTO
{
    public class CreateImageRequestDto
    {
        public string Url { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
    }

}
