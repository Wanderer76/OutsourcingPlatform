using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Service;

namespace OutsourcePlatformApp.Utils;

public class AccountVerificationMiddleware
{
    private readonly RequestDelegate next;

    public AccountVerificationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserService userService)
    {
        var token = context.Request.Headers.Authorization;
        if (token.Count != 0)
        {
            var username = JwtFormat.GetUsernameFromToken(token);
            var user = await userService.GetUserByUsername(username);
            if (!user.IsVerified)
            {
                if (context.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase)
                    && !context.Request.Path.Value!.Contains("auth", StringComparison.OrdinalIgnoreCase))
                    context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Аккаунт не активирован");
                return;
            }
        }

        await next.Invoke(context);
    }
}