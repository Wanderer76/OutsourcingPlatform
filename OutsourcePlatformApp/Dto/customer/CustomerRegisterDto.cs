using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class CustomerRegisterDto
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        [MinLength(6)] public string Password { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public string Inn { get; set; }
        public string Username { get; set; }
        [Phone] public string Phone { get; set; }
        [EmailAddress] public string Email { get; set; }

        public static Customer ConvertToCustomer(CustomerRegisterDto registerDto)
        {
            var user = new User
            {
                Username = registerDto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                SecondName = registerDto.SecondName,
                Phone = registerDto.Phone,
                Email = registerDto.Email,
                IsVerified = true,
            };
            user.Customer = new Customer
            {
                INN = registerDto.Inn,
                CompanyName = registerDto.OrganizationName,
                Address = registerDto.Address,
                UserId = user.Id,
                User = user
            };
            return user.Customer;
        }
    }
}