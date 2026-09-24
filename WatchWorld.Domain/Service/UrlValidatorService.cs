using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Service
{
    public class UrlValidatorService
    {
        public static class UrlValidator
        {
            private const int MaxUrlLength = 2048;

            private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp"
        };

            public static ImageUrlValidationResult ValidateImageUrl(string? url)
            {
                if (string.IsNullOrWhiteSpace(url))
                    return ImageUrlValidationResult.Failure("Den indtastede tekst er ikke en gyldig webadresse (URL).");

                url = url.Trim();

                if (url.Length > MaxUrlLength)
                    return ImageUrlValidationResult.Failure($"Webadressen er for lang (maks {MaxUrlLength} tegn).");

                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                    return ImageUrlValidationResult.Failure("Den indtastede tekst er ikke en gyldig webadresse (URL).");

                if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                {
                    return ImageUrlValidationResult.Failure(
                        $"Webadressen skal starte med http:// eller https://. Den nuværende protokol er '{uri.Scheme}://'.");
                }

                if (!string.IsNullOrEmpty(uri.UserInfo))
                    return ImageUrlValidationResult.Failure("Webadressen må ikke indeholde loginoplysninger.");

                string path = Uri.UnescapeDataString(uri.AbsolutePath);
                string extension = Path.GetExtension(path);

                if (!AllowedImageExtensions.Contains(extension))
                {
                    string allowedList = string.Join(", ", AllowedImageExtensions);
                    return ImageUrlValidationResult.Failure(
                        $"Billedformatet er ikke understøttet. URL'en skal ende på et af følgende formater: {allowedList}");
                }

                return ImageUrlValidationResult.Success();
            }
        }

    }
}
