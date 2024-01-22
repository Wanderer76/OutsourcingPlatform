using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;

namespace OutsourcePlatformApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerService customerService;
    private readonly PersonalAreaService personalAreaService;

    public CustomerController(CustomerService customerService, PersonalAreaService personalAreaService)
    {
        this.customerService = customerService;
        this.personalAreaService = personalAreaService;
    }

    [HttpGet("customer_detail_open/{customerId:int}")]
    public async Task<IActionResult> GetCustomerDetailCloseInfo([FromHeader(Name = "Authorization")] string? token,
        int customerId)
    {
        if (token == null)
            return Ok(await customerService.GetDetailCloseInfo(customerId));

        var executor = await personalAreaService.GetUserFromToken(token);

        if (executor.UserRoles.First().Name.Equals(UserRolesConstants.AdminRole))
        {
            return Ok(await customerService.GetDetailOpenInfo(customerId));
        }
        if (await customerService.IsCustomerConnectedToExecutor(executor.Executor, customerId))
        {
            return Ok(await customerService.GetDetailOpenInfo(customerId));
        }

        return Ok(await customerService.GetDetailCloseInfo(customerId));
    }

    [HttpGet("customer_orders")]
    public async Task<IActionResult> CustomerOrdersInfo(int customerId, int offset, int limit)
    {
        var orders = await customerService.GetCustomerOrders(customerId, offset, limit);
        return Ok(orders);
    }
}