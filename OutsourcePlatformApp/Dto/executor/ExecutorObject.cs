using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Utils;
using System.Text.Json.Serialization;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class ExecutorObject
    {
        [MinLength(1)] public string Surname { get; set; }
        [MinLength(1)] public string Name { get; set; }
        public string? SecondName { get; set; }
        [Phone] public string Phone { get; set; }
        [EmailAddress] public string Email { get; set; }

        [DataType(DataType.Date)]
        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly BirthDate { get; set; }

        public string? City { get; set; }
        public string Password { get; set; }

        public ExecutorObject(string surname, string name, string? secondName, string phone, string email,
            DateOnly birthDate, string? city, string password)
        {
            Surname = surname;
            Name = name;
            SecondName = secondName;
            Phone = phone;
            Email = email;
            BirthDate = birthDate;
            City = city;
            Password = password;
        }
    }
}