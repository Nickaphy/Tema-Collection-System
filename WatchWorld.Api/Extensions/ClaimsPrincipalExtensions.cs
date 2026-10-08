using System.Security.Claims;

namespace WatchWorld.Api.Extensions
{
    // Small helpers to read "who is calling" from the token
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            // The token calls it "sub", but ASP.NET renames it to NameIdentifier
            // when reading the token. We check both names to be safe.
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? user.FindFirst("sub")?.Value;

            return Guid.TryParse(value, out var userId) ? userId : Guid.Empty;
        }

        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole("Admin");
    }
}