using WatchWorld.Domain.Enums;
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class HighResImage : Aggregateroot
    {
	    public string Url { get; set; }
	    public int Width { get; set; }
	    public int Height { get; set; }

        private HighResImage() { }

        private HighResImage(string url, int width, int height)
        {
            Url = url;
            Width = width;
            Height = height;
        }
        public static HighResImage Create(string url, int width, int height)
        {
            var image = new HighResImage(url, width, height);
            Validate(url, width, height);
            return image;
        }

        public static HighResImage Delete(HighResImage image)
        {
            Validate(image.Url, image.Width, image.Height);
            return image;
        }

        public static void Validate(string url, int width, int height)
        {
            //Url validation
            var imageUrlValidationResult = UrlValidator.ValidateImageUrl(url);
            if (imageUrlValidationResult.IsValid == false)
                throw new UserInvalidInputException(imageUrlValidationResult.ErrorMessage);

            //Width and height validation
            if (width <= 0)
                throw new UserInvalidInputException("Billedet skal have en bredde");
            else if (width <= 500)
                throw new UserInvalidInputException("Bredden på et billede skal være over 500px");
            if (height <= 0)
                throw new UserInvalidInputException("Højden på et billede skal være over 0px.");
            else if (height <= 500)
                throw new UserInvalidInputException("Højden på et billede skal være over 500px");
        }

    public class ImageUrlValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class UrlValidator
    {
        public static ImageUrlValidationResult ValidateImageUrl(string url)
        {
            // 1. Validate that url isn't empty/null and is a valid URI
            if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
            {
                return new ImageUrlValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Den indtastede tekst er ikke en gyldig webadresse (URL)."
                };
            }

            // 2. Validate protocol
            string[] allowedProtocols = { Uri.UriSchemeHttp, Uri.UriSchemeHttps };
            if (!allowedProtocols.Contains(uriResult.Scheme))
            {
                return new ImageUrlValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Webadressen skal starte med http:// eller https://. Den nuværende protokol er '{uriResult.Scheme}://'."
                };
            }

            // 3. Validate format
            string path = uriResult.AbsolutePath;
            string[] validFormats = Enum.GetNames(typeof(ValidImageFormats));

            bool hasValidExtension = validFormats.Any(ext => path.EndsWith($".{ext}", StringComparison.OrdinalIgnoreCase));

            if (!hasValidExtension)
            {
                string allowedExtensionsList = string.Join(", ", validFormats.Select(f => $".{f}"));
                return new ImageUrlValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Billedformatet er ikke understøttet. URL'en skal ende på et af følgende formater: {allowedExtensionsList}"
                };
            }

            return new ImageUrlValidationResult { IsValid = true, ErrorMessage = null };
        }
    }



}

}
