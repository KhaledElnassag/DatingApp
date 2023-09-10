using DatingApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DatingApp.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var res = await next();

            if (!res.HttpContext.User.Identity.IsAuthenticated) return;
            var usernaem= res.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var _UserManger = res.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
            var user=await _UserManger.FindByNameAsync(usernaem);
            if (user is null) return;
            user.LastActive= DateTime.UtcNow;
            await _UserManger.UpdateAsync(user);
           

        }
    }
}
