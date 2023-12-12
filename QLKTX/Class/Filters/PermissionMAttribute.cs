using QLKTX.Class.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Enums;
using QLKTX.Class.Constants;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace QLKTX.Class.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionMAttribute : Attribute, IAuthorizationFilter
    {
        public string Action;
        private IMemoryCache _cache;
        //public PermissionAttribute(IMemoryCache cache)
        //{
        //    _cache = cache;
        //}
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            _cache = context.HttpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            if (!IsAuthorizedUser(context))
            {
                context.Result = new OkObjectResult(new AuthorizationErrorReponse
                {
                    Success = false,
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            else
            {
                var functionId = "";
                if (context.HttpContext.Request.Headers.ContainsKey("FunctionID"))
                {
                    context.HttpContext.Request.Headers.TryGetValue("FunctionID", out var hFunctionId);
                    functionId = hFunctionId;
                }

                // lấy functionId từ post form
                if (string.IsNullOrEmpty(functionId))
                {
                    string requestBody = "";
                    HttpRequest req = context.HttpContext.Request;
                    if (req.Body.CanSeek)
                    {
                        req.Body.Seek(0, SeekOrigin.Begin);
                        using StreamReader reader = new(req.Body, Encoding.UTF8, false, 1024, true);
                        requestBody = await reader.ReadToEndAsync();
                    }
                    req.Body.Position = 0;
                    functionId = HttpUtility.ParseQueryString(requestBody).Get("FunctionID");
                }
                // lấy functionId từ post form

                var user = (LoggedInUser)context.HttpContext.Items["UserLogin"];
                if (user == null)
                {
                    // not logged in or role not authorized
                    context.Result = new OkObjectResult(new AuthorizationErrorReponse
                    {
                        Success = false,
                        Message = "Unauthorized",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }
                else
                {
                    var permissionList = (List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + user.UserID);
                    UserPermiss permissions = permissionList?.Where(p => p.FunctionID == int.Parse(functionId)).FirstOrDefault();
                    // nếu là admin thì bỏ qua, ngược lại kiểm tra xem có fai là admin của nhóm ko, admin nhóm thì check theo nhóm
                    if (user.UserID.ToString() != "admin")
                    {
                        //1. Kiểm tra xem có phải admin nhóm hay user thường
                        bool exists;
                        exists = Enum.IsDefined(typeof(PermissionEnum), Action);     // exists = true
                        if (exists)
                        {
                            bool FAdm = user.FAdm;
                            if (permissions != null)
                            {
                                if (!FAdm)
                                {
                                    //user thường thì check                            
                                    if (!permissions.FView && Action == PermissionEnum.FView.ToString())
                                    {
                                        context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                        {
                                            Success = false,
                                            Message = MessageResources.PERMISS_00001,
                                            StatusCode = StatusCodes.Status403Forbidden
                                        });
                                    }
                                    else if (!permissions.FAdd && Action == PermissionEnum.FAdd.ToString())
                                    {
                                        context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                        {
                                            Success = false,
                                            Message = MessageResources.PERMISS_00002,
                                            StatusCode = StatusCodes.Status403Forbidden
                                        });
                                    }
                                    else if (!permissions.FEdit && Action == PermissionEnum.FEdit.ToString())
                                    {
                                        context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                        {
                                            Success = false,
                                            Message = MessageResources.PERMISS_00003,
                                            StatusCode = StatusCodes.Status403Forbidden
                                        });
                                    }
                                    else if (!permissions.FDel && Action == PermissionEnum.FDel.ToString())
                                    {
                                        context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                        {
                                            Success = false,
                                            Message = MessageResources.PERMISS_00004,
                                            StatusCode = StatusCodes.Status403Forbidden
                                        });
                                    }
                                    else if (!permissions.FApp && Action == PermissionEnum.FApp.ToString())
                                    {
                                        context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                        {
                                            Success = false,
                                            Message = MessageResources.PERMISS_00005,
                                            StatusCode = StatusCodes.Status403Forbidden
                                        });
                                    }
                                    else if (!permissions.FReject && Action == PermissionEnum.FReject.ToString())
                                    {
                                        context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                        {
                                            Success = false,
                                            Message = MessageResources.PERMISS_00015,
                                            StatusCode = StatusCodes.Status403Forbidden
                                        });
                                    }
                                }
                            }
                            else
                            {
                                context.Result = new OkObjectResult(new AuthorizationErrorReponse
                                {
                                    Success = false,
                                    Message = MessageResources.PERMISS_00014,
                                    StatusCode = StatusCodes.Status403Forbidden
                                });
                            }
                        }
                        else
                        {
                            context.Result = new OkObjectResult(new AuthorizationErrorReponse
                            {
                                Success = false,
                                Message = MessageResources.PERMISS_00013,
                                StatusCode = StatusCodes.Status403Forbidden
                            });
                        }
                    }
                }
            }
        }

        public static bool IsAuthorizedUser(AuthorizationFilterContext context)
        {
            try
            {
                return context.HttpContext.Items["UserLogin"] != null;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
