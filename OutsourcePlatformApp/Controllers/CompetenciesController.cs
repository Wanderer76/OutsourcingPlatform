using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetenciesController : ControllerBase
{
    private readonly CompetenciesService competenciesService;

    public CompetenciesController(CompetenciesService competenciesService)
    {
        this.competenciesService = competenciesService;
    }

    [HttpGet("categories/{offset:int}/{limit:int}")]
    public async Task<IActionResult> GetCategories(int offset, int limit)
    {
        var result = await competenciesService.GetCategories(offset, limit);
        return Ok(result);
    }

    [HttpGet("skills/{offset:int}/{limit:int}")]
    public async Task<IActionResult> GetSkills(int offset, int limit)
    {
        var result = await competenciesService.GetSkills(offset, limit);
        return Ok(result);
    } 
    [HttpGet("order-roles/{offset:int}/{limit:int}")]
    public async Task<IActionResult> GetOrderRoles(int offset, int limit)
    {
        var result = await competenciesService.GetOrderRoles(offset, limit);
        return Ok(result);
    }

    [HttpPost("skills/create")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task<SkillDto> CreateSkill(string name)
    {
        return await competenciesService.CreateSkill(name);
    }

    [HttpPost("categories/create")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task<CategoryDto> CreateCategory(string name)
    {
        return await competenciesService.CreateCategory(name);
    }

    [HttpPost("skills/delete/{id:int}")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task DeleteSkill(int id)
    {
        await competenciesService.DeleteSkill(id);
    }

    [HttpPost("categories/delete/{id:int}")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task DeleteCategory(int id)
    {
        await competenciesService.DeleteCategory(id);
    }
}