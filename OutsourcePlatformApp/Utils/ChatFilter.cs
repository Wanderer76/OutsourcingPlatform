using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.SignalR;
using OutsourcePlatformApp.Hubs.Chat;
using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;
using Shared;

namespace OutsourcePlatformApp.Hubs;

public class ChatFilter : IHubFilter
{
    private readonly UserService userService;
    private readonly IUserRepository userRepository;
    private readonly IChatRepository chatRepository;

    public ChatFilter(UserService userService, IUserRepository userRepository, IChatRepository chatRepository)
    {
        this.userService = userService;
        this.userRepository = userRepository;
        this.chatRepository = chatRepository;
    }

    public async ValueTask<object> InvokeMethodAsync(
        HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {
        Console.WriteLine($"Calling hub method '{invocationContext.HubMethodName}'");
        try
        {
            var methodName = invocationContext.HubMethodName;
            if (methodName.Equals("Send"))
            {
                var userToken = invocationContext.HubMethodArguments[1]!.ToString();
                var chatId = int.Parse(invocationContext.HubMethodArguments[2].ToString());
                var username = JwtUtil.GetClaimsFromToken(userToken)["username"];
                var sender = await userRepository.GetUserByUsernameAsync(username);
                if (await userService.IsUserBanned(sender.Username))
                    throw new Exception("Вы забаненны");
                var chat = await chatRepository.GetChatAsync(chatId);
                var receiver =
                    await (userRepository.GetUserByIdAsync(chat.User1Id)) == null
                        ? (await userRepository.GetUserByIdAsync(chat.User2Id))
                        : null;
                if (receiver != null && receiver.IsBanned)
                    throw new Exception("Вы не можете отправить сообщение заблокированному пользлователю");
            }

            return await next(invocationContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception calling '{invocationContext.HubMethodName}': {ex}");
            await invocationContext.Hub.Clients.Client(invocationContext.Context.ConnectionId)
                .SendCoreAsync("SendMessageNotFound", new[] { ex.Message });

            return ValueTask.CompletedTask;
        }
    }

    public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        return next(context);
    }

    public Task OnDisconnectedAsync(
        HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
    {
        return next(context, exception);
    }
}