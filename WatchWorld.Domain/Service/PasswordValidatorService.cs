using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Service
{
    public class PasswordValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public IReadOnlyList<string> Errors { get; }

        public PasswordValidationResult(IReadOnlyList<string> errors)
        {
            Errors = errors;
        }

        public static PasswordValidationResult Success() =>
            new(Array.Empty<string>());

        public static PasswordValidationResult Failure(IEnumerable<string> errors) =>
            new(errors.ToList());
    }

        public interface IPasswordValidatorService
        {
            
            PasswordValidationResult Validate(string? password, string? email, string? name);

            //Validates and throws UserInvalidInputException if invalid with relevant messages
            void ValidateAndThrow(string? password, string? email, string? name);
        }

        public class PasswordValidatorService : IPasswordValidatorService
        {
            private readonly PasswordPolicy _policy;

            public PasswordValidatorService(PasswordPolicy? policy = null)
            {
                _policy = policy ?? new PasswordPolicy();
            }

            public PasswordValidationResult Validate(string? password, string? email, string? name)
            {
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(password))
                {
                    errors.Add("Adgangskoden må ikke være tom.");
                    return PasswordValidationResult.Failure(errors);
                }

                if (password.Length < _policy.MinLength)
                    errors.Add($"Adgangskoden skal være mindst {_policy.MinLength} tegn lang.");

                if (password.Length > _policy.MaxLength)
                    errors.Add($"Adgangskoden må højst være {_policy.MaxLength} tegn lang.");

                if (_policy.RequireUppercase && !password.Any(char.IsUpper))
                    errors.Add("Adgangskoden skal indeholde mindst ét stort bogstav.");

                if (_policy.RequireLowercase && !password.Any(char.IsLower))
                    errors.Add("Adgangskoden skal indeholde mindst ét lille bogstav.");

                if (_policy.RequireDigit && !password.Any(char.IsDigit))
                    errors.Add("Adgangskoden skal indeholde mindst ét tal.");

                if (_policy.RequireSpecialChar && !password.Any(c => !char.IsLetterOrDigit(c)))
                    errors.Add("Adgangskoden skal indeholde mindst ét specialtegn (f.eks. !, @, # eller %).");

                if (_policy.DisallowWhitespace && password.Any(char.IsWhiteSpace))
                    errors.Add("Adgangskoden må ikke indeholde mellemrum.");

                if (_policy.DisallowedPasswords.Contains(password))
                    errors.Add("Denne adgangskode er for almindelig og let at gætte. Vælg en anden.");

                if (_policy.DisallowPersonalInfo && ContainsPersonalInfo(password, email, name, out var matchedFragment))
                errors.Add($"Adgangskoden må ikke indeholde dit navn eller din e-mailadresse ('{matchedFragment}').");


            return errors.Count == 0
                    ? PasswordValidationResult.Success()
                    : PasswordValidationResult.Failure(errors);
            }

        private bool ContainsPersonalInfo(string password, string? email, string? name, out string matchedFragment)
        {
            var fragments = new List<string>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                // Split on whitespace so "Anne Jensen" checks "Anne" and "Jensen" separately.
                fragments.AddRange(name.Split(
                    new[] { ' ', '\t', '-' },
                    StringSplitOptions.RemoveEmptyEntries));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var atIndex = email.IndexOf('@');
                var localPart = atIndex > 0 ? email[..atIndex] : email;

                // Split on common separators so "jane.doe" checks "jane" and "doe" separately.
                fragments.AddRange(localPart.Split(
                    new[] { '.', '_', '+', '-' },
                    StringSplitOptions.RemoveEmptyEntries));

                fragments.Add(localPart);
            }

            foreach (var fragment in fragments)
            {
                if (fragment.Length < _policy.MinPersonalInfoFragmentLength)
                    continue;

                if (password.Contains(fragment, StringComparison.OrdinalIgnoreCase))
                {
                    matchedFragment = fragment;
                    return true;
                }
            }

            matchedFragment = string.Empty;
            return false;
        }


        public void ValidateAndThrow(string? password, string? email, string? name)
            {
                var result = Validate(password, email, name);
                if (!result.IsValid)
                    throw new UserInvalidInputException(string.Join(" ", result.Errors));
            }
        }

    }
