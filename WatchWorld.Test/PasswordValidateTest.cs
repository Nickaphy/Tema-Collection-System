using WatchWorld.Domain.Service;
using Xunit;

namespace WatchWorld.Test;

public class PasswordValidatorTests
{
    private readonly PasswordValidatorService _validator = new();

    [Fact]
    public void Accepts_a_strong_password()
    {
        var result = _validator.Validate("Str0ng!Pass", "anna@test.dk", "Anna Jensen");

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("Ab1!")]            // for kort
    [InlineData("alllowercase1!")]  // intet stort bogstav
    [InlineData("ALLUPPERCASE1!")]  // intet lille bogstav
    [InlineData("NoDigits!!")]      // intet tal
    [InlineData("NoSpecial123")]    // intet specialtegn
    [InlineData("Has Space1!")]     // mellemrum
    public void Rejects_weak_passwords(string password)
    {
        var result = _validator.Validate(password, "anna@test.dk", "Anna Jensen");

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Rejects_password_containing_the_users_name()
    {
        var result = _validator.Validate("Anna123!x", "anna@test.dk", "Anna Jensen");

        Assert.False(result.IsValid);
    }
}