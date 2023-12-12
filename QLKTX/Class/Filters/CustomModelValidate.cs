using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;

namespace QLKTX.Class.Filters
{
    public class CustomModelValidate : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = string.Join(" | ", context.ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage));
                context.Result = new OkObjectResult(new AuthorizationErrorReponse
                {
                    Success = false,
                    Message = message,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
        }
    }
}
