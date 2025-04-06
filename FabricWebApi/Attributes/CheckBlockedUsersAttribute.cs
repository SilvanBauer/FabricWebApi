using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FabricWebApi;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CheckBlockedUsersAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context) { }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (context.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
        {
            var isBlockedClaim = context.HttpContext.User.Claims.SingleOrDefault(c => c.Type == CustomClaims.IsBlocked);
            if (isBlockedClaim != null && isBlockedClaim.Value == "True")
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
