using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;
using Shared;

namespace OutsourcePlatformApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutorController : ControllerBase
    {
        private readonly ExecutorService executorService;
        private readonly PersonalAreaService personalAreaService;
        private readonly ActionNotificationService notificationService;
        private readonly OrderService orderService;
        private readonly ChatRoomService chatRoomService;


        public ExecutorController(ExecutorService executorService, PersonalAreaService personalAreaService,
            ActionNotificationService notificationService, OrderService orderService, ChatRoomService chatRoomService)
        {
            this.executorService = executorService;
            this.personalAreaService = personalAreaService;
            this.notificationService = notificationService;
            this.orderService = orderService;
            this.chatRoomService = chatRoomService;
        }

        [HttpPost("executor_list/{offset:int}/{limit:int}")]
        public async Task<IActionResult> GetExecutorList(int offset, int limit,
            CompetenciesDto? competencies = null)
        {
            try
            {
                var result =
                    await executorService.GetExecutorList(offset, limit, competencies!.Skills,
                        competencies!.Categories);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("create_chatroom/{orderId:int}")]
        public async Task<IActionResult> CreateChatRoom(int orderId)
        {
            var customer = await orderService.GetCustomerUserByOrderId(orderId);
            var user = await personalAreaService.GetUserFromToken(
                HttpContext.Request.Headers.Authorization[0]);
            await chatRoomService.CreateChatRoom(customer, user, orderId);
            return Ok();
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("finish_project/{orderId:int}")]
        public async Task<IActionResult> FinishProject([FromHeader(Name = "Authorization")] string token, int orderId,
            bool isFinish)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                await executorService.FinishProject(user.Executor, orderId, isFinish);
                var customer = await orderService.GetCustomerUserByOrderId(orderId);
                if (isFinish)
                    await notificationService.CreateActionNotification(token, user, customer, orderId,
                        NotificationMessages.OrderCompleted);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("set_file_exists/{orderId:int}")]
        public async Task<IActionResult> SetFileProjectExists([FromHeader(Name = "Authorization")] string token,
            int orderId)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                await executorService.SetFileToResponse(user.Executor, orderId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("executor_detail/{executorId:int}")]
        public async Task<IActionResult> GetExecutorDetailCloseInfo([FromHeader(Name = "Authorization")] string? token,
            int executorId)
        {
            if (token == null)
                return Ok(await executorService.GetDetailCloseInfo(executorId));


            var customer = await personalAreaService.GetUserFromToken(token);
            if (customer.Customer != null &&
                await executorService.IsExecutorConnectedToCustomer(customer.Customer, executorId) ||
                customer.UserRoles.First().Name.Equals(UserRolesConstants.AdminRole))
            {
                return Ok(await executorService.GetDetailOpenInfo(executorId));
            }

            return Ok(await executorService.GetDetailCloseInfo(executorId));
        }


        [HttpGet("executor_reviews/{executorId:int}/{offset:int}/{limit:int}")]
        public async Task<IActionResult> ExecutorReviewsInfo(int executorId, int offset, int limit)
        {
            return Ok(await executorService.GetExecutorReviews(executorId, offset, limit));
        }
        
        [HttpGet("executor_reviews/{orderId}")]
        [Authorize]
        public async Task<IActionResult> ExecutorReviewsInfo(int orderId)
        {
            var user = await personalAreaService.GetUserFromToken(Request.Headers.Authorization!);
            return Ok(await executorService.GetExecutorReviews(user,orderId));
        }
        
    }
}