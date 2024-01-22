using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class CompetenciesRepository : ICompetenciesRepository
{
    private readonly ApplicationDbContext DbContext;

    public CompetenciesRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<Skill> GetSkillByIdAsync(int id)
    {
        return await DbContext.Skills.FirstAsync(skill => skill.Id == id);
    }

    public async Task<Skill> GetSkillByNameAsync(string name)
    {
        return await DbContext.Skills.FirstAsync(skill => skill.Name == name);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await DbContext.Categories.FirstAsync(category => category.Id == id);
    }

    public async Task<Category> GetCategoryByNameAsync(string name)
    {
        return await DbContext.Categories.FirstAsync(category => category.Name == name);
    }

    public async Task<List<Category>> GetCategoriesAsync(int offset, int limit)
    {
        return await DbContext.Categories
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Skill>> GetSkillsAsync(int offset, int limit)
    {
        return await DbContext.Skills
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Category> CreateCategoryAsync(string name)
    {
        var category = new Category(name);
        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();
        return category;
    }

    public async Task<Skill> CreateSkillAsync(string name)
    {
        var skill = new Skill(name);
        await DbContext.Skills.AddAsync(skill);
        await DbContext.SaveChangesAsync();
        return skill;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        DbContext.Categories.Remove(await DbContext.Categories.FirstAsync(category => category.Id == id));
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteSkillAsync(int id)
    {
        DbContext.Skills.Remove(await DbContext.Skills.FirstAsync(skill => skill.Id == id));
        await DbContext.SaveChangesAsync();
    }
}