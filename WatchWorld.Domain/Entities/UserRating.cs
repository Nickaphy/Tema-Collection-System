using System.ComponentModel.DataAnnotations;
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class UserRating : Aggregateroot
    {
        public Guid RatedTargetId { get; private set; }
        public int RatingAmount { get; private set; }
        public string Description { get; private set; }
        public Guid RatedByUserId { get; private set; }
        public bool IsRatingWatch { get; private set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;


        private UserRating() { }

        public UserRating(Guid ratedToUserId, int ratingAmount, string description, bool? isRatingWatch, Guid ratedByUserId)
        {
            RatedTargetId = ratedToUserId;
            RatingAmount = ratingAmount;
            Description = description;
            RatedByUserId = ratedByUserId;
        }

        public static UserRating Create(Guid ratedToUserId, int ratingAmount, string description, bool? isRatingWatch, Guid ratedByUserId)
        {
            var rating = new UserRating(ratedToUserId, ratingAmount, description, isRatingWatch, ratedByUserId);
            return rating;
        }
        public static UserRating Update(Guid specificUserRatingId, int ratingAmount, string description)
        {
            var rating = new UserRating
            {
                Id = specificUserRatingId,
                RatingAmount = ratingAmount,
                Description = description
            };
            return rating;
        }
        public static UserRating Delete(Guid specificUserRatingId)
        {
            var rating = new UserRating
            {
                Id = specificUserRatingId
            };
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
