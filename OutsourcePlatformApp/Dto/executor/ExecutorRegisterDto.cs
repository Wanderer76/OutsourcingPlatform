using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class ExecutorRegisterDto
    {
        [MinLength(1)] public string Surname { get; set; }
        [MinLength(1)] public string Name { get; set; }
        public string? SecondName { get; set; }
        [MinLength(6)] public string Password { get; set; }
        [DataType(DataType.Date)] public DateTime BirthDate { get; set; }
        public string City { get; set; }
        [Phone] public string Phone { get; set; }
        [MinLength(1)] public string Username { get; set; }
        [EmailAddress] public string Email { get; set; }

        public static Executor ConvertToExecutor(ExecutorRegisterDto registerDto)
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
            };
            user.Executor = new Executor
            {
                Birthdate = new DateOnly(registerDto.BirthDate.Year, registerDto.BirthDate.Month,
                    registerDto.BirthDate.Day),
                City = registerDto.City,
                User = user,
                UserId = user.Id
            };
            return user.Executor;
        }
    }
}