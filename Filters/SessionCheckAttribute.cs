using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ERPAPP.Filters
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();

            if (controller == "Login")
                return;

            var session = context.HttpContext.Session;

            if (session == null || session.GetInt32("UserRowId") == null)
            {
                bool isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjax)
                {
                    context.Result = new ContentResult
                    {
                        Content = "ERPSystemSessionExpired",
                        ContentType = "text/plain"
                    };
                }
                else
                {
                    context.HttpContext.Session.SetString("ERPSystemSessionExpired", "1");

                    context.Result = new RedirectToActionResult(
                        "Index",
                        "Login",
                        null
                    );
                }

                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
