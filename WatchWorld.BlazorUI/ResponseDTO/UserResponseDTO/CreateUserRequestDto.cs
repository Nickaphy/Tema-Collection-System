namespace WatchWorld.BlazorUI.ResponseDTO.UserResponseDTO
{
    public class CreateUserRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public List<object> Rating { get; set; } = new();
    }

}
