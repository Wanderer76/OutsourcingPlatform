using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Service;

namespace OutsourcePlatformApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("executor/register")]
        public async Task<IActionResult> Register(ExecutorRegisterDto executorRegister)
        {
            try
            {
                await authService.RegisterExecutorAsync(executorRegister);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("customer/register")]
        public async Task<IActionResult> Register(CustomerRegisterDto executorRegister)
        {
            try
            {
                await authService.RegisterCustomerAsync(executorRegister);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthRequestDto authRequest)
        {
            try
            {
                var result = await authService.Authenticate(authRequest);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("verify/{verifyCode:guid}")]
        public async Task<IActionResult> VerifyAccount(
            Guid verifyCode)
        {
            try
            {
                return Ok(await authService.VerifyAccount(verifyCode));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromForm] string refreshToken)
        {
            try
            {
                return Ok(await authService.UpdateToken(refreshToken));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}