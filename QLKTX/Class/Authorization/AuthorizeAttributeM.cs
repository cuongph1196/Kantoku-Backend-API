using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Entities;
using QLKTX.Class.Enums;

namespace QLKTX.Class.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttributeM : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;

        public AuthorizeAttributeM(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var isCheckDevice = (bool)context.HttpContext.Items["IsCheckDevice"];
            if (!isCheckDevice)
            {
                context.Result = new OkObjectResult(new AuthorizationErrorReponse
                {
                    Success = false,
                    Message = "Thiết bị chưa được kích hoạt",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
                return;
            }
            // authorization
            var user = (LoggedInUser)context.HttpContext.Items["UserLogin"];
            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                // not logged in or role not authorized
                context.Result = new OkObjectResult(new AuthorizationErrorReponse
                {
                    Success = false,
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
                return;
            }
            //phải check xem token có phải của user đăng nhập này không mới được.
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (IsAuthenticated)
            {
                var identityId = context.HttpContext.User.Identity.Name;
                if (identityId != user.UserID)
                {
                    // token đúng nhưng không phải của user login này
                    context.Result = new OkObjectResult(new AuthorizationErrorReponse
                    {
                        Success = false,
                        Message = "Mã xác thực này không phải của người dùng hiện tại",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                    return;
                }
            }
        }
    }
}
