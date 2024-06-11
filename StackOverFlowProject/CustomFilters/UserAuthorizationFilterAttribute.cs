using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StackOverFlowProject.CustomFilters
{
    public class UserAuthorizationFilterAttribute : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //context.Session.GetString("CurrentUserName")
            var session = context.HttpContext.Session;
            if (session.GetString("CurrentUserName") == null)
            {
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}
