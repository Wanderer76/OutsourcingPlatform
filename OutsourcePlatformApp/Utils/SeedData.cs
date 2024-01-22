using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Utils;

public class SeedData
{
    private readonly ApplicationDbContext DbContext;

    public SeedData(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void SetSeedData()
    {
        foreach (var i in GetExecutors())
            DbContext.Add(i);
        foreach (var i in GetCustomers())
            DbContext.Add(i);
        foreach (var i in GetAdmins())
            DbContext.Add(i);
        DbContext.SaveChanges();
    }


    private IEnumerable<Admin> GetAdmins()
    {
        return new List<Admin>
        {
            new()
            {
                User = new User
                {
                    UserRoles = new List<UserRole>
                        { DbContext.UserRoles.First(role => role.Name == UserRolesConstants.AdminRole) },
                    Username = "@admin1",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Name = "Админ",
                    Surname = " ",
                    SecondName = "",
                    Email = "ateplinsky@mail.ru",
                    Executor = null,
                    Customer = null,
                    IsVerified = true,
                    Admin = new Admin()
                    {
                    },
                    Phone = "",
                    RefreshToken = new RefreshToken
                    {
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                    }
                }
            }
        };
    }

    private IEnumerable<Executor> GetExecutors()
    {
        return new List<Executor>
        {
            new()
            {
                City = "Екатеринбург",
                Birthdate = DateOnly.FromDateTime(new DateTime(2002, 1, 1)),
                Categories = DbContext.Categories.Take(5).ToList(),
                Skills = DbContext.Skills.Take(10).ToList(),
                Educations = new List<Education>
                {
                    new("УРФУ", "Программная инженерия", 2024),
                    new("Skillbox", "java разработчик", 2024),
                },

                User = new User
                {
                    UserRoles = new List<UserRole>
                        { DbContext.UserRoles.First(role => role.Name == UserRolesConstants.ExecutorRole) },
                    Username = "@user1",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Name = "Егор",
                    Surname = "Коновалов",
                    SecondName = "Тимурович",
                    Email = "probawossefu-4685@yopmail.com",
                    Phone = "89539804554",
                    IsVerified = true,
                    RefreshToken = new RefreshToken
                    {
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        //  IsBaned = false,
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                    }
                }
            },
            new()
            {
                City = "Екатеринбург",
                Birthdate = DateOnly.FromDateTime(new DateTime(2002, 1, 1)),
                Educations = new List<Education>
                {
                    new("УРФУ", "Программная инженерия", 2024),
                    new("Skillbox", "java разработчик", 2024),
                },
                User = new User
                {
                    UserRoles = new List<UserRole>
                        { DbContext.UserRoles.First(role => role.Name == UserRolesConstants.ExecutorRole) },
                    Username = "@user2",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Name = "Владимир",
                    Surname = "Семенов",
                    SecondName = "Филиппович",
                    Email = "xinoifrepabra-8823@yopmail.com",
                    Phone = "89532461654",
                    IsVerified = true,
                    Reviews = null,
                    RefreshToken = new RefreshToken
                    {
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                    }
                }
            },
            new()
            {
                City = "Екатеринбург",
                Birthdate = DateOnly.FromDateTime(new DateTime(2002, 1, 1)),
                Categories = DbContext.Categories.Take(5).ToList(),
                Skills = DbContext.Skills.Skip(10).Take(10).ToList(),
                Educations = new List<Education>
                {
                    new("УРФУ", "Программная инженерия", 2024),
                    new("Skillbox", "java разработчик", 2024),
                },
                User = new User
                {
                    UserRoles = new List<UserRole>
                        { DbContext.UserRoles.First(role => role.Name == UserRolesConstants.ExecutorRole) },
                    Username = "@user3",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Name = "Никита",
                    Surname = "Куликов",
                    SecondName = "Сергеевич",
                    Email = "yucredeubeyo-5207@yopmail.com",
                    Phone = "89531742370",
                    IsVerified = true,
                    //UserContacts = new Contact("", "nick_zts", "https://github.com/MrNickev", null),
                    Reviews = null,
                    RefreshToken = new RefreshToken
                    {
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                    }
                }
            }
        };
    }

    private IEnumerable<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new()
            {
                INN = "029186207952",
                Address = "пл. Кирова, Екатеринбург, Свердловская обл.",
                CompanyName = "Sed Sem Corporation",

                User = new User
                {
                    Username = "@user4",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Name = "Анна",
                    Surname = "Васильева",
                    SecondName = "Романовна",
                    Email = "ginibraunaji-1198@yopmail.com",
                    Phone = "89537580396",
                    IsVerified = true,
                    RefreshToken = new RefreshToken
                    {
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                    },
                    UserRoles = new List<UserRole>
                        { DbContext.UserRoles.First(role => role.Name == UserRolesConstants.CustomerRole) },
//                     UserContacts = new Contact(
//                         @"66 Бит — профессиональный разработчик программного обеспечения, специализирующийся на разработке под заказ нетиповых IT проектов, ориентированных на международные рынки.
//
// Мы разрабатываем сложные приложения, создаем удобные, технологичные и современные веб-сервисы и мобильные приложения.",
//                         "nick_zts", "https://github.com/MrNickev", null),
                    Reviews = null,
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            CompanyName = "Sed Sem Corporation",
                            Deadline = DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
                            IsCompleted = false,
                            IsPublished = true,
                            Name =
                                @"Скрипт для автоматического создания новых профилей в MS Oulook – авто импорт данных из Excel",
                            Description =
                                @"Скрипт, который автоматически создает новые профили в MS outlook 2010, импортируя данные из файла Excel (логин и IMAP пароль).
Либо Ваш вариант удобного сборщика писем с других ящиков по протоколу IMAP.
Главная конечная задача – получение писем ящиков по протоколу IMAP на 1 сборщик писем. Загрузка данных почтовых ящиков (логины и пароли для IMAP) из файла Excel.
",
                            Price = 0,
                            OrderCategories = DbContext.Categories.Take(5).ToList(),
                            OrderSkills = DbContext.Skills.Take(5).ToList(),
                            OrderVacancies =
                                DbContext.OrderRoles.Take(5).ToList().Select(x => new OrderVacancy
                                {
                                    OrderRole = x,
                                    MaxWorkers = 1
                                }).ToList()
                        }
                    },
                }
            },
            new()
            {
                INN = "046113626042",
                Address = "пл. Кирова, Екатеринбург, Свердловская обл.",
                CompanyName = "Interdum Limited",
                User = new User
                {
                    Username = "@user5",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Name = "Полина",
                    Surname = "Платонова",
                    SecondName = "Леонидовна",
                    Email = "xeuneihecrolli-4229@yopmail.com",
                    Phone = "89537772212",
                    IsVerified = true,
                    Customer = new Customer
                    {
                        INN = "046113626042",
                        Address = "пл. Кирова, Екатеринбург, Свердловская обл.",
                        CompanyName = "Interdum Limited",
                    },
                    RefreshToken = new RefreshToken
                    {
                        CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                        Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                    },
                    UserRoles = new List<UserRole>
                        { DbContext.UserRoles.First(role => role.Name == UserRolesConstants.CustomerRole) },
//                     UserContacts = new Contact(
//                         @"66 Бит — профессиональный разработчик программного обеспечения, специализирующийся на разработке под заказ нетиповых IT проектов, ориентированных на международные рынки.
//
// Мы разрабатываем сложные приложения, создаем удобные, технологичные и современные веб-сервисы и мобильные приложения.",
//                         "nick_zts", "https://github.com/MrNickev", null),
                    Reviews = null
                }
            },
        };
    }
}