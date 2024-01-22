using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Dto.order;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;
using Shared;

namespace OutsourcePlatformApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly PersonalAreaService personalAreaService;
        private readonly ResponseService responseService;
        private readonly ExecutorService executorService;
        private readonly ActionNotificationService notificationService;
        private readonly ChatRoomService chatService;

        public OrderController(OrderService orderService, PersonalAreaService personalAreaService,
            ResponseService responseService, ExecutorService executorService,
            ActionNotificationService notificationService, ChatRoomService chatService)
        {
            this.orderService = orderService;
            this.personalAreaService = personalAreaService;
            this.responseService = responseService;
            this.executorService = executorService;
            this.notificationService = notificationService;
            this.chatService = chatService;
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("response/{orderId:int}")]
        public async Task<IActionResult> ResponseToOrder([FromHeader(Name = "Authorization")] string token, int orderId,
            int vacancyId, bool reaction = true)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                if (reaction)
                {
                    await orderService.CreateResponse(user.Executor!, orderId, vacancyId);
                    var customer = await orderService.GetCustomerUserByOrderId(orderId);

                    await notificationService.CreateActionNotification(token,user,
                        customer,
                        orderId, NotificationMessages.ResponseCreate);
                }
                else
                {
                    await orderService.RemoveResponse(user.Executor!, orderId);
                }

                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("create-chat-room")]
        public async Task<IActionResult> CreateChatRoom([FromHeader(Name = "Authorization")] string token, int orderId)
        {
            var sender = await personalAreaService.GetUserFromToken(token);
            var customer = await orderService.GetCustomerUserByOrderId(orderId);
            if (sender.Id == customer.Id)
                return BadRequest("Вы не можете начать диалог сами с собой");
            var result = await chatService.CreateChatRoom(customer, sender, orderId);
            return Ok(new
            {
                result.Id
            });
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("update_response")]
        public async Task<IActionResult> ReactToResponse([FromHeader(Name = "Authorization")] string token,
            bool isAccept, int vacancyId, int responseId)
        {
            try
            {
                await orderService.UpdateResponse(isAccept, vacancyId, responseId);
                var user = await personalAreaService.GetUserFromToken(token);
                var executor = await responseService.GetExecutorByResponseId(responseId);
                var order = await responseService.GetOrderByResponseId(responseId);
                await notificationService.CreateActionNotification(token,
                    user,
                    executor.User,
                    order.Id,
                    isAccept ? NotificationMessages.ResponseConfirm : NotificationMessages.ResponseRefuse);

                if (isAccept)
                    await chatService.CreateChatRoom(user, executor.User, order.Id);

                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpDelete("delete_executor")]
        public async Task<IActionResult> DeleteExecutorFromOrder([FromHeader(Name = "Authorization")] string token,
            DeleteExecutorDto deleteExecutorDto)
        {
            try
            {
                await orderService.DeleteExecutorFromOrder(deleteExecutorDto);
                var user = await personalAreaService.GetUserFromToken(token);
                await notificationService.CreateActionNotification(token,
                    await personalAreaService.GetUserFromToken(token),
                    (await executorService.GetExecutorById(deleteExecutorDto.ExecutorId)).User,
                    deleteExecutorDto.OrderId,
                    NotificationMessages.RemoveFromOrder);
                var chatRoom = await chatService.GetUserChat(user, deleteExecutorDto.ExecutorId);
                await chatService.CreateMessage(user.Id, chatRoom.Id, deleteExecutorDto.Message);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpGet("user_created_orders/{offset:int}/{limit:int}")]
        public async Task<IActionResult> GetCustomerOrders([FromHeader(Name = "Authorization")] string token,
            int offset, int limit)
        {
            var user = await personalAreaService.GetUserFromToken(token);
            var result = await orderService.GetUserOrders(user, offset, limit);
            return Ok(result);
        }

        [Authorize(Roles = "executor_role")]
        [HttpGet("executor_orders/{offset:int}/{limit:int}")]
        public async Task<IActionResult> GetExecutorOrders([FromHeader(Name = "Authorization")] string token,
            int offset, int limit)
        {
            var user = await personalAreaService.GetUserFromToken(token);
            var result = await orderService.GetExecutorOrders(user, offset, limit);
            return Ok(result);
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromHeader(Name = "Authorization")] string token,
            OrderCreationDto creationDto)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                await orderService.CreateOrder(user, creationDto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("publish_order/{orderId:int}")]
        public async Task<IActionResult> PublishOrder([FromHeader(Name = "Authorization")] string token, int orderId)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                await orderService.PublishOrder(user, orderId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpDelete("delete/{orderId:int}")]
        public async Task<IActionResult> DeleteOrder([FromHeader(Name = "Authorization")] string token,
            int orderId)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                await orderService.DeleteOrder(user, orderId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("update/{orderId:int}")]
        public async Task<IActionResult> UpdateOrder([FromHeader(Name = "Authorization")] string token, int orderId,
            OrderCreationDto order)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                var updated = orderService.UpdateOrder(user, orderId, order);
                return Ok(await updated);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("finish_project/{orderId:int}")]
        public async Task<IActionResult> FinishProject([FromHeader(Name = "Authorization")] string token, int orderId)
        {
            var user = await personalAreaService.GetUserFromToken(token);
            await orderService.FinishProject(user, orderId);
            return Ok();
        }


        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpGet("detail_order_info/{orderId:int}")]
        public async Task<IActionResult> GetDetailOrderInfo([FromHeader(Name = "Authorization")] string token,
            int orderId)
        {
            try
            {
                var user = await personalAreaService.GetUserFromToken(token);
                var result = await orderService.GetDetailOrderInfo(user, orderId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("order_list/{offset:int}/{limit:int}")]
        public async Task<IActionResult> GetOrderList([FromHeader(Name = "Authorization")] string? token, int offset,
            int limit,
            CompetenciesDto? competencies = null)
        {
            try
            {
                User? user = null;
                if (token != null)
                    user = await personalAreaService.GetUserFromToken(token);
                var result =
                    await orderService.GetOrderList(user, offset, limit, competencies!.Skills,
                        competencies!.Categories);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetCommonOrderInfo([FromHeader(Name = "Authorization")] string? token,
            int orderId)
        {
            try
            {
                User? user;
                if (token != null)
                    user = await personalAreaService.GetUserFromToken(token);
                else
                    user = null;
                var result = await orderService.GetCommonOrderInfo(user, orderId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize(Roles = $"{UserRolesConstants.CustomerRole},{UserRolesConstants.ExecutorRole}")]
        [HttpPost("create_review")]
        public async Task<IActionResult> CreateReview([FromHeader(Name = "Authorization")] string token,
            ReviewCreationDto reviewCreationDto)
        {
            try
            {
                await orderService.CreateReview(reviewCreationDto);
                var customer = await personalAreaService.GetUserFromToken(token);
                var executor = await executorService.GetExecutorById(reviewCreationDto.ExecutorId);
                await notificationService.CreateActionNotification(token,
                    customer,
                    executor.User,
                    reviewCreationDto.OrderId, NotificationMessages.ReviewCreate);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}