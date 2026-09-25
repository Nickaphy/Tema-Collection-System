using WatchWorld.Domain.Enums;
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;
using static WatchWorld.Domain.Service.UrlValidatorService;

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

            //Width and height validation
            if (width <= 0)
                throw new UserInvalidInputException("Billedet skal have en bredde over 0px");
            else if (width <= 500)
                throw new UserInvalidInputException("Bredden på et billede skal være over 500px");
            if (height <= 0)
                throw new UserInvalidInputException("Højden på et billede skal være over 0px.");
            else if (height <= 500)
                throw new UserInvalidInputException("Højden på et billede skal være over 500px");

            //Url validation
            var imageUrlValidationResult = UrlValidator.ValidateImageUrl(url);
            if (imageUrlValidationResult.IsValid == false)
                throw new UserInvalidInputException($"Der skete en fejl under billed validering: {imageUrlValidationResult.ErrorMessage}");
        }

    }

}
