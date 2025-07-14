using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperMarketApi.Services;
using SuperMarketApi.Models;

namespace SuperMarketApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private IEnumerable<RoleEnum> _roles;
        public AuthorizationAttribute(params RoleEnum[] roles)
        {
            _roles = roles.ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Get the token from the Authorization header
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            // Get the user service from DI
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            // Check authorization
            bool authorized = false;
            try
            {
                authorized = userService.Authorize(token, _roles.ToArray());
            }
            catch (UnauthorizedAccessException)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!authorized)
            {
                // Return a custom 403 result to avoid authentication scheme errors
                context.Result = new ObjectResult("Forbidden") { StatusCode = 403 };
            }
        }
    }
}
