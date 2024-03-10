using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TheThao.Models.Authentication
{
    public class Authentication : ActionFilterAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Authentication(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("UserId") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Controller", "Login" },
                        {"Action", "Logout" }
                    });
            }
        }
    }
}
