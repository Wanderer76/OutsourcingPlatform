using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Dto;
using System.Collections;
using System.Text.Json.Serialization;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class CustomerUpdateDto
    {
        [MinLength(1)] public string Surname { get; set; }
        [MinLength(1)] public string Name { get; set; }
        public string? SecondName { get; set; }
        [Phone] public string Phone { get; set; }
        [MinLength(5)] public string Address { get; set; }
        [EmailAddress] public string Email { get; set; }
        public string Inn { get; set; }
        public string CompanyName { get; set; }
        public string Password { get; set; }

        public CustomerUpdateDto(string surname, string name, string? secondName, string phone, string address, string email, string inn, string companyName, string password)
        {
            Surname = surname;
            Name = name;
            SecondName = secondName;
            Phone = phone;
            Address = address;
            Email = email;
            Inn = inn;
            CompanyName = companyName;
            Password = password;
        }
    }
}