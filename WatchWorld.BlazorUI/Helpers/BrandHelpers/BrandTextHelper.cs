using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WatchWorld.BlazorUI.Helpers.BrandHelpers
{
    public static class BrandTextHelper
    {
        // "A. Lange & Söhne" -> "a-lange-sohne". Idempotent: slugging a slug returns it unchanged,
        // so it's safe to compare Slug(brand.Name) against a route parameter that's already a slug.
        public static string Slug(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            var lower = value.Trim().ToLowerInvariant()
                .Replace("æ", "ae").Replace("ø", "oe").Replace("ß", "ss");

            string decomposed;
            try { decomposed = lower.Normalize(NormalizationForm.FormD); }
            catch (PlatformNotSupportedException) { decomposed = lower; }

            var sb = new StringBuilder(decomposed.Length);
            foreach (var c in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return Regex.Replace(sb.ToString(), "[^a-z0-9]+", "-").Trim('-');
        }

        // Monogram shown when a brand has no logo URL.
        public static string Initials(string name)
        {
            var parts = name.Split(new[] { ' ', '-', '.' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => "?",
                1 => parts[0][..1].ToUpperInvariant(),
                _ => $"{parts[0][..1]}{parts[1][..1]}".ToUpperInvariant()
            };
        }
    }
}