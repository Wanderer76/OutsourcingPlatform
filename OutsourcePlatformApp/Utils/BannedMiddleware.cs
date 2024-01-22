using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Service;

namespace OutsourcePlatformApp.Utils;

public class BannedMiddleware
{
    private readonly RequestDelegate next;

    public BannedMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserService userService, IUserRepository userRepository)
    {
        var token = context.Request.Headers.Authorization;
        if (token.Count != 0)
        {
            var username = JwtFormat.GetUsernameFromToken(token);
            if (await userService.IsUserBanned(username) &&
                (context.Request.Method != "GET" || context.Request.Path.Equals("/chats")))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Вы забанены");
                return;
            }
        }

        await next.Invoke(context);
    }
}