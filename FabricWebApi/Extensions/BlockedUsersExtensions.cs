using FabricWebApi.Middlewares;

namespace FabricWebApi.Extensions;

public static class BlockedUsersExtensions
{
    public static IApplicationBuilder UseBlockedUsersMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockedUsersMiddleware>();
    }
}
