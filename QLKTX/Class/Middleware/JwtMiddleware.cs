using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Constants;
using QLKTX.Class.Entities;
using QLKTX.Class.Exceptions;
using QLKTX.Class.Repository;
using System.Security.Claims;
using System.Text.Json;

namespace QLKTX.Class.Middleware
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly IJwtUtils _jwtUtils;
        private readonly IPosDeviceRepository _posDeviceRepository;

        public JwtMiddleware(IJwtUtils jwtUtils,
            IPosDeviceRepository posDeviceRepository)
        {
            _jwtUtils = jwtUtils;
            _posDeviceRepository = posDeviceRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var deviceCode = context.Request.Headers["DeviceCode"].FirstOrDefault()?.Split(" ").Last();
            var isCheckDevice = _posDeviceRepository.CheckDevice(deviceCode);
            if (isCheckDevice.Success)
            {
                context.Items["IsCheckDevice"] = true;
                var principal = _jwtUtils.GetPrincipalFromExpiredToken(token);
                if (principal != null)
                {
                    var claims = principal.Identities.First().Claims.ToList();
                    if (claims.Count > 0)
                    {
                        string userID = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value; //userID
                        if (userID is not null)
                        {
                            var user = JsonSerializer.Deserialize<LoggedInUser>(claims.First(x => x.Type == "UserLogin").Value);
                            // attach user to context on successful jwt validation
                            context.Items["UserLogin"] = user;
                        }
                    }
                }
            }
            else
            {
                context.Items["IsCheckDevice"] = false;
            }
            await next(context);
        }
    }
}
