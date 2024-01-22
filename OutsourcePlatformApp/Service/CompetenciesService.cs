using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class CompetenciesService
{
    private readonly ICompetenciesRepository competenciesRepository;
    private readonly IOrderRoleRepository orderRoleRepository;

    public CompetenciesService(ICompetenciesRepository competenciesRepository, IOrderRoleRepository orderRoleRepository)
    {
        this.competenciesRepository = competenciesRepository;
        this.orderRoleRepository = orderRoleRepository;
    }
    public async Task<List<OrderRoleDto>> GetOrderRoles(int offset, int limit)
    {
        var categories = await orderRoleRepository
            .GetOrderRoles(offset, limit);
        return categories.Select(category => new OrderRoleDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }
    public async Task<List<CategoryDto>> GetCategories(int offset, int limit)
    {
        var categories = await competenciesRepository
            .GetCategoriesAsync(offset, limit);
        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }

    public async Task<List<SkillDto>> GetSkills(int offset, int limit)
    {
        var categories = await competenciesRepository
            .GetSkillsAsync(offset, limit);
        return categories.Select(category => new SkillDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }

    public async Task<SkillDto> CreateSkill(string name)
    {
        var result = await competenciesRepository.CreateSkillAsync(name);
        return new SkillDto
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public async Task<CategoryDto> CreateCategory(string name)
    {
        var result = await competenciesRepository.CreateCategoryAsync(name);
        return new CategoryDto
        {
            Id = result.Id,
            Name = result.Name
        };
    }

    public async Task DeleteSkill(int id)
    {
        await competenciesRepository.DeleteSkillAsync(id);
    }

    public async Task DeleteCategory(int id)
    {
        await competenciesRepository.DeleteCategoryAsync(id);
    }
}