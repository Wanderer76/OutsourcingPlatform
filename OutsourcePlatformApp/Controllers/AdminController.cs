using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AdminService adminService;
    private readonly UserService userService;

    public AdminController(AdminService adminService, UserService userService)
    {
        this.adminService = adminService;
        this.userService = userService;
    }

    [HttpGet("all_users/{offset:int}/{limit:int}")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task<IEnumerable<UserDto>> GetUsers(bool? isBanned, int offset, int limit)
    {
        return await adminService.GetUsers(isBanned, offset, limit);
    }

    [HttpPost("change_account_status")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task ChangeAccountStatus([FromBody] BanRequest banRequest)
    {
        await adminService.ChangeAccountStatus(banRequest);
    }

    [HttpGet("detail_info")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task<IActionResult> GetDetailUserInfo(string username)
    {
        var user = await userService.GetUserByUsername(username);
        return user.Executor != null
            ? Ok(await adminService.GetExecutorInfo(user))
            : Ok(await adminService.GetCustomerInfo(user));
    }

    [HttpGet("users_system_info")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public async Task<IActionResult> GetSystemInfo()
    {
        return Ok(await adminService.GetSystemUserInfo());
    }
}