
using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class User : Aggregateroot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string? Note { get; private set; }
        public string Password { get; private set; }
        public bool IsAdmin { get; private set; }
        public List<UserRating> Rating { get; private set; }


        private User() { }

        private User(
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string address,
            string city,
            string? note,
            string password,
            bool isAdmin,
            List<UserRating> rating)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            City = city;
            Note = note;
            Password = password;
            IsAdmin = isAdmin;
            Rating = rating ?? new List<UserRating>();
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new UserInvalidInputException($"Du skal have et fornavn!");

            if (string.IsNullOrWhiteSpace(LastName))
                throw new UserInvalidInputException($"Du skal have et efternavn!");

            if (string.IsNullOrWhiteSpace(PhoneNumber) 
                || PhoneNumber.Length < 8
                || !PhoneNumber.All(char.IsDigit))
                throw new UserInvalidInputException($"Du skal udfylde et gyldigt telefonnummer!");

            if (string.IsNullOrWhiteSpace(Email))
                throw new UserInvalidInputException($"Du skal udfylde din email!");

            if (string.IsNullOrWhiteSpace(Address))
                throw new UserInvalidInputException($"Du skal udfylde din Adresse!");

            if (string.IsNullOrWhiteSpace(City))
                throw new UserInvalidInputException($"Du skal udfylde din by!");

            var validator = new PasswordValidatorService();
            validator.ValidateAndThrow(Password, Email, $"{FirstName} {LastName}");

        }

        public static User Create(
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string address,
            string city,
            string? note,
            string password,
            List<UserRating> rating)
        {
            var user = new User(firstName, lastName, phoneNumber, email, address, city, note, password, false, rating);

            return user;
        }
        public void SetAdmin(
            User user)
        {
            if (user.IsAdmin == true)
                throw new UserInvalidInputException($"Brugeren er allerede en Admin");
            else if (user.Id == Guid.Empty)
                throw new UserInvalidInputException($"Brugeren skal have et gyldigt ID");
            else
                return;
        }

        public bool IsUserAdmin()
        {
            return IsAdmin;
        }

        public void UpdateUser(
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string address,
            string city,
            string? note,
            string password
            )
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            City = city;
            Note = note;
            Password = password;
            Validate();
        }
    }
}
