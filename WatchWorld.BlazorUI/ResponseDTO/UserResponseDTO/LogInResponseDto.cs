namespace WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO
{
    // What the API sends back after a successful login:
    // all the normal user fields (inherited from UserDto) plus the token
    public class LogInResponseDto : UserDto
    {
        public string Token { get; set; } = string.Empty;
    }
}