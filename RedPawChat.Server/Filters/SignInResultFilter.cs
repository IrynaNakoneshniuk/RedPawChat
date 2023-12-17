using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace RedPawChat.Server.Filters;

public class SignInResultFilter : Attribute, IAsyncResultFilter
{

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var claims = context.HttpContext.User.Claims.ToList();

        var identity = new ClaimsIdentity(claims, "RedPawAuth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        await context.HttpContext.SignInAsync("RedPawAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = true });

        await next();
    }
}
