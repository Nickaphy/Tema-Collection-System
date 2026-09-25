namespace WatchWorld.Domain.ValueObjects
{
    public class PasswordPolicy
    {
        public int MinLength { get; set; } = 8;
        public int MaxLength { get; set; } = 128;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireSpecialChar { get; set; } = true;
        public bool DisallowWhitespace { get; set; } = true;
        public bool DisallowPersonalInfo { get; set; } = true;
        public int MinPersonalInfoFragmentLength { get; set; } = 3;
        public HashSet<string> DisallowedPasswords { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            "password", 
            "password1", 
            "12345678", 
            "qwerty123", 
            "admin123"
        };
    }
}
