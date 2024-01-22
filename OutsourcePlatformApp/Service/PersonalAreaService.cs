using System.Text.Json;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OutsourcePlatformApp.Service;

public class PersonalAreaService
{
    private readonly IUserRepository userRepository;
    private readonly ICompetenciesRepository competenciesRepository;
    private readonly IExecutorRepository executorRepository;
    private readonly IOrderRepository orderRepository;

    public PersonalAreaService(IUserRepository userRepository, ICompetenciesRepository competenciesRepository,
        IExecutorRepository executorRepository, IOrderRepository orderRepository)
    {
        this.userRepository = userRepository;
        this.competenciesRepository = competenciesRepository;
        this.executorRepository = executorRepository;
        this.orderRepository = orderRepository;
    }

    public async Task<User> GetUserFromToken(string token)
    {
        try
        {
            var username = JwtFormat.GetUsernameFromToken(token);
            var user = await userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                throw new ArgumentException("Пользователя с таким ником не существует");
            return await Task.Run(() => user);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
    }

    public async Task<string> GetPersonalArea(string token)
    {
        var user = await GetUserFromToken(token);
        return await SerializePersonalArea(user);
    }

    private async Task<string> SerializePersonalArea(User? user)
    {
        if (user == null)
            throw new ArgumentException("Пользователя с ником " + user.Username + " не существует");
        var jsonString = "";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        if (user.UserRoles.Any(role => role.Name == "executor_role"))
        {
            var dto = new ExecutorInfoDto(user,
                await executorRepository.GetExecutorFinishProjects(user.Executor.Id));
            jsonString = JsonSerializer.Serialize(dto, options);
        }
        else if (user.UserRoles.Any(role => role.Name == "customer_role"))
        {
            var orders = await orderRepository.GetUserOrdersAsync(user, 0, int.MaxValue);
            var dto = new CustomerInfoDto(user, orders);
            jsonString = JsonSerializer.Serialize(dto, options);
        }

        return await Task.Run(() => jsonString);
    }

    private async Task<string> SerializeExecutor(User? user)
    {
        if (user == null)
            throw new ArgumentException("Пользователя с ником " + user.Username + " не существует");
        var jsonString = "";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (user.UserRoles.Any(role => role.Name == "executor_role"))
        {
            var dto = new ExecutorObject(user.Surname, user.Name, user.SecondName, user.Phone,
                user.Email, user.Executor.Birthdate, user.Executor.City, user.Password);
            jsonString = JsonSerializer.Serialize(dto, options);
        }

        return await Task.Run(() => jsonString);
    }


    public async Task<string> UpdateExecutor(ExecutorObject updatedExecutor, string token)
    {
        var username = JwtFormat.GetUsernameFromToken(token);
        var user = await userRepository.GetUserByUsernameAsync(username);
        if (user is null)
            throw new ArgumentException("Пользователя с таким ником не существует");

        if (user.UserRoles.All(role => role.Name != "executor_role"))
            throw new ArgumentException("Пользователь с таким ником не является исполнителем");

        user.Surname = updatedExecutor.Surname;
        user.Name = updatedExecutor.Name;
        user.SecondName = updatedExecutor.SecondName;
        user.Email = updatedExecutor.Email;
        user.Phone = updatedExecutor.Phone;

        user.Executor.Birthdate = updatedExecutor.BirthDate;
        user.Executor.City = updatedExecutor.City;
        if (!string.IsNullOrWhiteSpace(updatedExecutor.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(updatedExecutor.Password);

        await userRepository.UpdateUserInfoAsync();
        return await SerializeExecutor(user);
    }

    private async Task<string> SerializeCustomer(User? user)
    {
        if (user == null)
            throw new ArgumentException("Пользователя с ником " + user.Username + " не существует");
        var jsonString = "";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (user.UserRoles.Any(role => role.Name == "customer_role"))
        {
            var dto = new CustomerUpdateDto(user.Surname, user.Name, user.SecondName, user.Phone,
                user.Customer!.Address,
                user.Email, user.Customer.INN, user.Customer.CompanyName, user.Password);
            jsonString = JsonSerializer.Serialize(dto, options);
        }

        return await Task.Run(() => jsonString);
    }

    public async Task<string> EditCustomer(CustomerUpdateDto customerDto, string token)
    {
        var user = await GetUserFromToken(token);
        if (!InnCheck.CheckInn(customerDto.Inn))
            throw new ArgumentException("Неправильный Инн");

        if (user.UserRoles.All(role => role.Name != "customer_role"))
            throw new ArgumentException("Пользователь с таким ником не является заказчиком");

        user.Surname = customerDto.Surname;
        user.Name = customerDto.Name;
        user.SecondName = customerDto.SecondName;
        user.Email = customerDto.Email;
        user.Phone = customerDto.Phone;
        user.Customer!.Address = customerDto.Address;
        user.Customer.CompanyName = customerDto.CompanyName;
        user.Customer.INN = customerDto.Inn;
        if (!string.IsNullOrWhiteSpace(customerDto.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(customerDto.Password);

        await userRepository.UpdateUserInfoAsync();
        return await SerializeCustomer(user);
    }

    public async Task<ContactsUpdateDto> UpdateContacts(ContactsUpdateDto contacts, string token)
    {
        var username = JwtFormat.GetUsernameFromToken(token);
        var user = await userRepository.GetUserByUsernameAsync(username);
        if (user is null)
            throw new ArgumentException("Пользователя с таким ником не существует");
        user.UserContacts = new Contact
        {
            About = contacts.About,
            ContactLinks = contacts.Contacts.Select(x => new ContactLink { Name = x.Name, Url = x.Url }).ToList()
        };

        await userRepository.UpdateUserInfoAsync();
        return await Task.Run(() => contacts);
    }

    private async Task<string> SerializeCategories(User? user)
    {
        if (user == null)
            throw new ArgumentException("Пользователя с ником " + user.Username + " не существует");
        var jsonString = "";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (user.UserRoles.Any(role => role.Name == "executor_role"))
        {
            var dto = new CategoriesDto(user.Executor);
            jsonString = JsonSerializer.Serialize(dto, options);
        }

        return await Task.Run(() => jsonString);
    }

    public async Task<string> EditCategories(List<CategoryDto> categories, string token)
    {
        var user = await GetUserFromToken(token);
        var updatedCategories = new List<Category>();
        categories.ForEach(c => updatedCategories.Add(competenciesRepository.GetCategoryByNameAsync(c.Name).Result));
        user.Executor!.Categories = updatedCategories;
        await userRepository.UpdateUserInfoAsync();

        var categoriesUpdated = await SerializeCategories(user);
        return await Task.Run(() => categoriesUpdated);
    }

    private async Task<string> SerializeSkills(User? user)
    {
        if (user == null)
            throw new ArgumentException("Пользователя с ником " + user.Username + " не существует");
        var jsonString = "";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (user.UserRoles.Any(role => role.Name == "executor_role"))
        {
            var dto = new SkillsDto(user.Executor);
            jsonString = JsonSerializer.Serialize(dto, options);
        }

        return await Task.Run(() => jsonString);
    }

    public async Task<string> EditSkills(List<SkillDto> skills, string token)
    {
        var user = await GetUserFromToken(token);
        var updatedSkills = new List<Skill>();
        skills.ForEach(s => updatedSkills.Add(competenciesRepository.GetSkillByNameAsync(s.Name).Result));
        user.Executor!.Skills = updatedSkills;
        await userRepository.UpdateUserInfoAsync();

        var skillsUpdated = await SerializeSkills(user);
        return await Task.Run(() => skillsUpdated);
    }

    private async Task<string> SerializeEducations(User? user)
    {
        if (user == null)
            throw new ArgumentException("Пользователя с ником " + user.Username + " не существует");
        var jsonString = "";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (user.UserRoles.Any(role => role.Name == "executor_role"))
        {
            var dto = new EducationsDto(user.Executor);
            jsonString = JsonSerializer.Serialize(dto, options);
        }

        return await Task.Run(() => jsonString);
    }

    private List<Education> CreateEducationsList(IEnumerable<EducationUpdateDto> list)
    {
        var updatedList = list.Select(updatedEducation =>
                new Education(updatedEducation.Place,
                    updatedEducation.Speciality, updatedEducation.GraduationYear))
            .ToList();

        return updatedList;
    }

    [Obsolete]
    public async Task<List<Education>> GetEducationsList(Executor executor, List<EducationUpdateDto> list)
    {
        var executorEducations = await userRepository.GetExecutorEducations(executor);
        if (!executorEducations.Any())
            return CreateEducationsList(list);

        foreach (var updatedEducation in list)
        {
            if (executorEducations.All(ed => ed.Id != updatedEducation.Id))
            {
                executorEducations.Append(new Education(updatedEducation.Place,
                    updatedEducation.Speciality, updatedEducation.GraduationYear));
            }

            var oldEducation = await userRepository.GetEducationToUpdate(updatedEducation);
            oldEducation.Place = updatedEducation.Place;
            oldEducation.Speciality = updatedEducation.Speciality;
            oldEducation.GraduationYear = updatedEducation.GraduationYear;
        }

        return await Task.Run(() => executor.Educations) ?? throw new ArgumentNullException();
    }

    public async Task<string> UpdateEducations(List<EducationUpdateDto> educations, string token)
    {
        var username = JwtFormat.GetUsernameFromToken(token);
        var user = await userRepository.GetUserByUsernameAsync(username);
        if (user is null)
            throw new ArgumentException("Пользователя с таким ником не существует");

        var userEducations = user.Executor.Educations ?? new List<Education>();
        var newEducations = new List<Education>();
        foreach (var education in educations)
        {
            if (education.Id != null)
            {
                var current = userEducations.Find(e => e.Id == education.Id);
                if (current == null)
                    throw new ArgumentNullException($@"Данного образования не существует {education.Speciality}");

                current.GraduationYear = education.GraduationYear;
                current.Speciality = education.Speciality;
                current.Place = education.Place;
                newEducations.Add(current);
            }
            else
            {
                newEducations.Add(new Education(education.Place, education.Speciality, education.GraduationYear));
            }
        }

        user.Executor.Educations = newEducations;
        await userRepository.UpdateUserInfoAsync();
        return await SerializeEducations(user);
    }
}