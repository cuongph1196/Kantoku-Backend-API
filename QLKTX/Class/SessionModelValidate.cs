using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QLKTX.Class
{
    public class SessionModelValidate : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessions = context.HttpContext.Session.GetString("UserSession");
            if (sessions == null)
            {
                context.Result = new RedirectToActionResult("SessionTimeout", "Home", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
