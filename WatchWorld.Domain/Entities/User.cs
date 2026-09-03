using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Linq;
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
            Id = Guid.NewGuid();

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

            if (string.IsNullOrWhiteSpace(Password)
                || Password.Length < 8
                || !Password.Any(Char.IsUpper)
                || !Password.Any(Char.IsLower)
                || !Password.Any(Char.IsDigit))
                throw new UserInvalidInputException($"Du skal oprette et gyldigt adgangskode!");

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

        public static User CreateAdmin(
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
            var admin = new User(firstName, lastName, phoneNumber, email, address, city, note, password, true, rating);

            return admin;
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
