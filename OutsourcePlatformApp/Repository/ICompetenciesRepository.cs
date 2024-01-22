using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface ICompetenciesRepository
{
    Task<Skill> GetSkillByIdAsync(int id);
    Task<Skill> GetSkillByNameAsync(string name);
    Task<Category> GetCategoryByIdAsync(int id);
    Task<Category> GetCategoryByNameAsync(string name);
    Task<List<Category>> GetCategoriesAsync(int offset, int limit);
    Task<List<Skill>> GetSkillsAsync(int offset, int limit);
    Task<Category> CreateCategoryAsync(string name);
    Task<Skill> CreateSkillAsync(string name);
    Task DeleteCategoryAsync(int id);
    Task DeleteSkillAsync(int id);

}