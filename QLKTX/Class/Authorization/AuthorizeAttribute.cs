using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class.Entities;
using QLKTX.Class.Enums;
using System.Data;

namespace QLKTX.Class.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;
        private IMemoryCache _cache;
        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? new Role[] { };
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;
            // authorization
            var user = context.HttpContext.Session.GetString(SystemConstants.UserSession) != null
                ? JsonConvert.DeserializeObject<LoggedInUser>(context.HttpContext.Session.GetString(SystemConstants.UserSession)) : null;
            if (user == null || (_roles.Any() && !_roles.Contains(user.Role))) // || !roleAuth(user.UserID, _role)
            {
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Home",
                        action = "Login"
                    })
                );
                return;
            }

            //check permiss view page
            if (user.UserID != "admin")
            {
                var functionIdL = context.HttpContext.Request.Query["FunctionID"];
                if (functionIdL.Count > 0)
                {
                    var functionId = functionIdL[0];
                    var permissionList = (List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + user.UserID);
                    UserPermiss permissions = permissionList?.Where(p => p.FunctionID == int.Parse(functionId)).FirstOrDefault();
                    //user thường thì check                            
                    if (permissions == null || !permissions.FView)
                    {
                        context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "Forbidden"
                            })
                        );
                    }
                    return;
                }
            }
        }

    }
}
