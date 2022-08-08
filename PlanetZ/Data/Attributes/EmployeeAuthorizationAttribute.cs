using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlanetZ.Data.Attributes
{
    public class EmployeeAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return;
            }
            var userManager = context.HttpContext.RequestServices.GetService<UserManager<Employee>>();
            var employee = userManager.Users.FirstOrDefault(x => x.Id == context.HttpContext.Session.GetString("userId"));

            if (employee.Role == Enums.EnumRoles.CompanyManager || employee.Role == Enums.EnumRoles.SiteManager)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
