using QLKTX.Class.Authorization;
using System.Globalization;

namespace QLKTX.Class.Middleware.UserAccountSession
{
    public class UserAccountSessionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var userL = context.Session.GetString(SystemConstants.AccountLogin);
            Mession.AccountLogin = string.IsNullOrEmpty(userL) ? null : userL;
            await next(context);
        }
    }
}
