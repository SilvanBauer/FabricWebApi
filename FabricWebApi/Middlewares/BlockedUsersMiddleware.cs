using FabricWebApi.Services;
using System.Security.Claims;

namespace FabricWebApi.Middlewares;

public class BlockedUsersMiddleware
{
    private readonly RequestDelegate _next;

    public BlockedUsersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IBlockedUsersService blockedUsersService)
    {
        if (context?.User?.Identity?.IsAuthenticated ?? false)
        {
            var currentUsername = context.User.Identity.Name;
            var isBlocked = blockedUsersService.IsUserBlocked(currentUsername);
            var claimsIdentity = new ClaimsIdentity(context.User.Identity, [new Claim(CustomClaims.IsBlocked, isBlocked.ToString())]);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            context.User = claimsPrincipal;
        }

        await _next(context);
    }
}
