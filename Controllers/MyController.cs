using System.Linq;
using System.Web.Mvc;
using UserIdentity.Attributes;

namespace UserIdentity.Controllers
{
    public class MyController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Account", action = "Login" }
                    )
                );
                return;
            }

            // Check for the Unauthorize attribute on the action method
            var actionDescriptor = filterContext.ActionDescriptor;
            var unauthorizeAttribute = actionDescriptor.GetCustomAttributes(typeof(UnauthorizeAttribute), false)
                                                       .FirstOrDefault() as UnauthorizeAttribute;

            if (unauthorizeAttribute != null)
            {
                var deniedRoles = unauthorizeAttribute.Roles;

                // If the user is in one of the denied roles, redirect to unauthorized page
                if (deniedRoles.Any(role => user.IsInRole(role)))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new { controller = "Home", action = "Unauthorized" }
                        )
                    );
                    return;
                }
            }

            // If no unauthorized roles or conditions are met, continue with the action execution
            base.OnActionExecuting(filterContext);
        }

       
    }
}