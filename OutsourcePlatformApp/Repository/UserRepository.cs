using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext DbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<Executor> CreateExecutorAsync(Executor registerDto)
    {
        var isUsernameExists =
            await DbContext.Users.FirstOrDefaultAsync(user => user.Username.Equals(registerDto.User.Username));
        if (isUsernameExists != null)
            throw new ArgumentException("Пользователь с таким ником уже существует");

        registerDto.User.UserRoles = new List<UserRole>
            { DbContext.UserRoles.First(role => role.Name == "executor_role") };

        registerDto.User.RefreshToken = new RefreshToken
        {
            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
            //  IsBaned = false,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        };
        await DbContext.AddAsync(registerDto);
        await DbContext.SaveChangesAsync();
        return registerDto;
    }

    public async Task<Customer> CreateCustomerAsync(Customer registerDto)
    {
        var isCustomerExists = DbContext.Customers
            .FirstOrDefaultAsync(customer1 => customer1.INN.Equals(registerDto.INN));

        if (await isCustomerExists != null)
            throw new ArgumentException("Компания с этим ИНН уже существует");

        if (await DbContext.Users.FirstOrDefaultAsync(user1 => user1.Username == registerDto.User.Username) != null)
            throw new ArgumentException("Пользователь с таким ником уже существует");

        registerDto.User.UserRoles = new List<UserRole>
            { DbContext.UserRoles.First(role => role.Name == "customer_role") };
        registerDto.User.RefreshToken = new RefreshToken
        {
            CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            Expires = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
            //IsBaned = false,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        };
        await DbContext.AddAsync(registerDto);
        await DbContext.SaveChangesAsync();
        return registerDto;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        DbContext.Users.Update(user);
        await DbContext.SaveChangesAsync();
        return user;
    }

    public Task DeleteUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetUsers(int offset, int limit)
    {
        return await DbContext.Users
            .Include(user => user.UserRoles)
            .Where(user => user.Executor != null || user.Customer != null)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<User>> FilterUsers(bool? isBanned, int offset, int limit)
    {
        return await DbContext.Users
            .Include(user => user.UserRoles)
            .Where(user => user.IsBanned == isBanned && (user.Executor != null || user.Customer != null))
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await DbContext.Users
            .Where(user => user.Id == id)
            .Include(user => user.UserRoles)
            .Include(user => user.Executor)
            .Include(user => user.Executor!.Educations)
            .Include(user => user.Executor!.Skills)
            .Include(user => user.Executor!.Categories)
            .Include(user => user.Customer)
            .Include(user => user.UserContacts)
            .ThenInclude(user => user.ContactLinks)
            .Include(user => user.RefreshToken)
            .Include(user => user.Reviews)
            .FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsersByIdAsync(List<int> ids)
    {
        return await DbContext.Users
            .Where(user => ids.Contains(user.Id))
            .Include(user => user.UserRoles)
            .Include(user => user.Executor)
            .Include(user => user.Executor!.Educations)
            .Include(user => user.Executor!.Skills)
            .Include(user => user.Executor!.Categories)
            .Include(user => user.Customer)
            .Include(user => user.UserContacts)
            .Include(user => user.RefreshToken)
            .Include(user => user.Reviews)
            .ToListAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await DbContext.Users
            .Where(user => user.Username == username)
            .Include(user => user.UserRoles)
            .Include(user => user.Executor)
            .Include(user => user.Executor!.Educations)
            .Include(user => user.Executor!.Skills)
            .Include(user => user.Executor!.Categories)
            .Include(user => user.Customer)
            .Include(user => user.Orders)
            .Include(user => user.UserContacts)
            .ThenInclude(contact => contact.ContactLinks)
            .Include(user => user.RefreshToken)
            .Include(user => user.Reviews)
            .FirstOrDefaultAsync();
    }

    public Task<IQueryable<Education>> GetExecutorEducations(Executor executor)
    {
        return Task.Run(() => DbContext.Educations.Where(ed => ed.Executor.Equals(executor)));
    }

    public Task<Education> GetEducationToUpdate(EducationUpdateDto updatedEducation)
    {
        return Task.Run(() => DbContext.Educations.First(ed => ed.Id.Equals(updatedEducation.Id)));
    }

    public async Task<User> GetUserByExecutorIdAsync(int id)
    {
        var t = await DbContext.Executors.Where(executor => executor.Id == id).FirstAsync();
        return await GetUserByIdAsync(t.UserId);
    }

    public async Task<User> GetUserByCustomerIdAsync(int id)
    {
        var t = await DbContext.Customers.Where(executor => executor.Id == id).FirstAsync();
        return await GetUserByIdAsync(t.UserId);
    }

    public async Task<User> GetCommonUserByExecutorIdAsync(int id)
    {
        return await DbContext.Users.Where(user => user.Executor.Id == id)
            .Include(user => user.UserRoles)
            .Include(user => user.RefreshToken)
            .FirstAsync();
    }

    public async Task<User> GetCommonUserByCustomerIdAsync(int id)
    {
        return await DbContext.Users.Where(user => user.Customer.Id == id)
            .Include(user => user.UserRoles)
            .Include(user => user.RefreshToken)
            .FirstAsync();
    }

    public async Task<User> GetUserByRefreshToken(string refreshToken)
    {
        return await DbContext.Users
            .Include(user => user.RefreshToken)
            .Include(user => user.UserRoles)
            .Where(user => user.RefreshToken.Token == refreshToken)
            .FirstAsync();
    }

    public async Task UpdateUserInfoAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken> GetRefreshTokenByUserId(int userId)
    {
        return await DbContext.RefreshTokens.FirstAsync(token => token.Id == userId);
    }

    public async Task<bool> IsBannedByUsernameAsync(string username)
    {
        var user = await DbContext.Users.FirstAsync(user => user.Username.Equals(username));
        return user.IsBanned;
    }

    public async Task<User> GetUserbyActivationCode(Guid code)
    {
        return await DbContext.Users
            .Where(user => user.ActivationTokens.Any(x => x.Token == code))
            .Include(user => user.UserRoles)
            .Include(user => user.Executor)
            .Include(user => user.Executor!.Educations)
            .Include(user => user.Executor!.Skills)
            .Include(user => user.Executor!.Categories)
            .Include(user => user.Customer)
            .Include(user => user.UserContacts)
            .Include(user => user.RefreshToken)
            .Include(user => user.Reviews)
            .FirstOrDefaultAsync();
    }
}