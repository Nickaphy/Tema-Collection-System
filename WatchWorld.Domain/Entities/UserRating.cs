using System.ComponentModel.DataAnnotations;
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class UserRating : Aggregateroot
    {
        public Guid RatedToUserId { get; private set; }
        public int RatingAmount { get; private set; }
        public string Description { get; private set; }
        public Guid RatedByUserId { get; private set; }


        private UserRating() { }

        private UserRating(Guid ratedToUserId, int ratingAmount, string description, Guid ratedByUserId)
        {
            RatedToUserId = ratedToUserId;
            RatingAmount = ratingAmount;
            Description = description;
            RatedByUserId = ratedByUserId;
        }

        private static UserRating Create(Guid ratedToUserId, int ratingAmount, string description, Guid ratedByUserId)
        {
            var rating = new UserRating(ratedToUserId, ratingAmount, description, ratedByUserId);
            return rating;
        }

        public static void Validate(Guid ratedToUserId,int ratingAmount, Guid ratedByUserId)
        {
            if (ratedToUserId == Guid.Empty)
                throw new UserInvalidInputException($"Ugyldigt, mangler en bruger-ID for den bedømte bruger.");

            if (ratedByUserId == Guid.Empty)
                throw new UserInvalidInputException($"Ugyldigt, mangler en bruger-ID for den bedømmende bruger");

            if (ratingAmount < 0 || ratingAmount > 5)
                throw new UserInvalidInputException($"En bedømmelse skal være mellem 0 og 5.");

        }

    }
}
