using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Entities;
using QLKTX.Class.Exceptions;
using QLKTX.Class.Filters;
using QLKTX.Class.Helper;
using QLKTX.Class.Repository;
using QLKTX.Class.Service;
using QLKTX.Class.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthorizeAttributeM = QLKTX.Class.Authorization.AuthorizeAttributeM;

namespace QLKTX.Controllers.Mobile
{
    [Route("api/UserAccountM")]
    [ApiController]
    [AuthorizeAttributeM]
    public class UserAccountMController : BaseMController
    {
        private readonly ILogger<UserAccountMController> _logger;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IPosDeviceRepository _posDeviceRepository;
        private readonly IUserAccountService _userAccountService;
        private readonly IConfiguration _configuration;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private IMemoryCache _cache;

        public UserAccountMController(ILogger<UserAccountMController> logger,
            IConfiguration configuration,
            IMemoryCache cache,
            IUserAccountRepository userAccountRepository,
            IUserPermissionRepository userPermissionRepository,
            IPosDeviceRepository posDeviceRepository,
            IUserAccountService userAccountService,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
            _userAccountRepository = userAccountRepository;
            _userPermissionRepository = userPermissionRepository;
            _posDeviceRepository = posDeviceRepository;
            _userAccountService = userAccountService;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        [CustomModelValidate]
        public async Task<IActionResult> Login([FromBody] UserLoginMVm request)
        {
            var isCheckDevice = _posDeviceRepository.CheckDevice(request.DeviceCode);
            if (isCheckDevice.Success)
            {
                var objLogin = new UserLoginVm
                {
                    UserID = request.UserID,
                    Password = request.Password
                };
                var result = _userAccountRepository.AuthenticateM(objLogin);
                if (result.Success)
                {
                    var userVm = new UserLoginResultVm();
                    userVm = result.Data;
                    //session login
                    HttpContext.Session.SetString(SystemConstants.AccountLogin, userVm.UserID);
                    if ((List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermissionM + "_" + userVm.UserID) == null)
                    {
                        var permiss = _userPermissionRepository.GetPermissionByUser(userVm.UserID);
                        if (permiss.Success)
                        {
                            _cache.Set<dynamic>(SystemConstants.UserPermissionM + "_" + userVm.UserID, (List<UserPermiss>)permiss.Data.Items);
                        }
                    }

                    // authentication successful so generate jwt and refresh tokens
                    JwtSecurityToken jwtSecurityToken = _jwtUtils.GenerateJwtToken(userVm);
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    var validAccessTokenExpires = jwtSecurityToken.ValidTo.ToString();
                    DateTime convertedDate = DateTime.SpecifyKind(DateTime.Parse(validAccessTokenExpires), DateTimeKind.Utc);
                    DateTime dtAccessTokenExpires = convertedDate.ToLocalTime();
                    userVm.AccessTokenExpires = dtAccessTokenExpires;
                    // save changes to db by Email
                    //nếu có user có refresh token rồi thì lấy ra. còn chưa có thì gen lại
                    //refreshtoken = null là đã hết hạn rồi thì gen mới lại. còn hạn khỏi
                    if (userVm.RefreshToken == null)
                    {
                        var refreshToken = _jwtUtils.CreateRefreshToken();
                        userVm.RefreshToken = refreshToken.Token;
                        userVm.RefreshTokenExpires = refreshToken.Expires;
                        // save changes to db by Email
                        _userAccountRepository.UpdateRefreshToken(userVm);
                    }
                    return Ok(new ApiSuccessResult<LoggedInUser>(new LoggedInUser(userVm, jwtToken, userVm.RefreshToken)));
                }
                else
                {
                    //ViewBag.error = result.Message;
                    return Ok(new ApiErrorResult<LoggedInUser>(result.Message));
                }
            }
            else
            {
                return Ok(isCheckDevice);
            }
        }


        //vì khi access token hết hạn sẽ gởi kèm refresh token để lấy token mới. nên phải để AllowAnonymous
        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        [CustomModelValidate]
        public IActionResult RefreshToken([FromBody] TokenVm tokenModel)
        {
            var response = _userAccountService.RefreshToken(tokenModel);
            return Ok(new ApiSuccessResult<RefreshTokenResponseVm>(response));
        }


        

    }
}
