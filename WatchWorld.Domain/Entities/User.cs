using System;
using System.Collections.Generic;
using System.Text;
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
        public string Note { get; private set; }
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
            string note,
            string password,
            bool isAdmin,
            List<UserRating> rating)
        {
            Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(firstName))
                throw new UserInvalidInputException($"Du skal have et fornavn!");
            FirstName = firstName;
            if (string.IsNullOrWhiteSpace(lastName))
                throw new UserInvalidInputException($"Du skal have et efternavn!");
            LastName = lastName;
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new UserInvalidInputException($"Du skal udfylde dit telefonnummer!");
            PhoneNumber = phoneNumber;
            if (string.IsNullOrWhiteSpace(email))
                throw new UserInvalidInputException($"Du skal udfylde din email!");
            Email = email;
            if (string.IsNullOrWhiteSpace(address))
                throw new UserInvalidInputException($"Du skal udfylde din Adresse!");
            Address = address;
            if (string.IsNullOrWhiteSpace(city))
                throw new UserInvalidInputException($"Du skal udfylde din by!");
            City = city;
            Note = note;
            if (string.IsNullOrWhiteSpace(password))
                throw new UserInvalidInputException($"Du skal oprette en adgangskode!");
            Password = password;
            IsAdmin = isAdmin;
            List<UserRating> Rating = rating;
        }

        public static User Create(
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string address,
            string city,
            string note,
            string password,
            bool isAdmin,
            List<UserRating> rating)
        {
            var user = new User(firstName, lastName, phoneNumber, email, address, city, note, password, isAdmin, rating);

            return user;
        }


    }
}
