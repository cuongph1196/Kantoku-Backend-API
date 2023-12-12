using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Models;
using System.Diagnostics;
using System.Security.Claims;
using QLKTX.Class.ViewModels;
using QLKTX.Class.Repository;
using QLKTX.Class.Dtos;
using QLKTX.Class.Entities;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Authorization;
using AuthorizeAttribute = QLKTX.Class.Authorization.AuthorizeAttribute;
using System.Globalization;

namespace QLKTX.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration,
            IMemoryCache cache,
            IUserAccountRepository userAccountRepository,
            IUserPermissionRepository userPermissionRepository,
            IMenuRepository menuRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
            _userAccountRepository = userAccountRepository;
            _userPermissionRepository = userPermissionRepository;
            _menuRepository = menuRepository;
        }

        public IActionResult Index()
        {
            LoggedInUser userLogin = HttpContext.Session.GetString(SystemConstants.UserSession) != null
                ? JsonConvert.DeserializeObject<LoggedInUser>(HttpContext.Session.GetString(SystemConstants.UserSession)) : null;
            if (userLogin != null)
            {
                if ((List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + userLogin.UserID) == null)
                {
                    var permiss = _userPermissionRepository.GetPermissionByUser(userLogin.UserID);
                    if (permiss.Success)
                    {
                        _cache.Set<dynamic>(SystemConstants.UserPermission + "_" + userLogin.UserID, (List<UserPermiss>)permiss.Data.Items);
                    }
                }

                var moduleMenu = new FunctionMenuViewModel();
                var modules = HttpContext.Session.GetString(SystemConstants.ModuleMenuSession);
                if (modules != null)
                {
                    moduleMenu.ModuleMenus = JsonConvert.DeserializeObject<List<ModuleMenuVm>>(modules);
                }
                else
                {
                    var result = _menuRepository.GetModuleMenu();
                    if (result.Success)
                    {
                        moduleMenu.ModuleMenus = (List<ModuleMenuVm>)result.Data.Items;
                        HttpContext.Session.SetString(SystemConstants.ModuleMenuSession, JsonConvert.SerializeObject(moduleMenu.ModuleMenus));
                    }
                    
                    var resultFunc = _menuRepository.GetAllFunction();
                    if (resultFunc.Success)
                    {
                        var funcAll = (List<FunctionMenuVm>)resultFunc.Data.Items;
                        HttpContext.Session.SetString(SystemConstants.FunctionSession, JsonConvert.SerializeObject(funcAll));
                    }

                }
                return View(moduleMenu);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _cache.Remove(SystemConstants.UserPermission + "_" + HttpContext.Session.GetString(SystemConstants.AccountLogin));
            //HttpContext.Session.Clear();
            HttpContext.Session.Remove(SystemConstants.UserSession);
            HttpContext.Session.Remove(SystemConstants.UserPermission);
            HttpContext.Session.Remove(SystemConstants.ModuleMenuSession);
            HttpContext.Session.Remove(SystemConstants.FunctionSession);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            _cache.Remove(SystemConstants.UserPermission + "_" + HttpContext.Session.GetString(SystemConstants.AccountLogin));
            //HttpContext.Session.Clear();
            HttpContext.Session.Remove(SystemConstants.UserSession);
            HttpContext.Session.Remove(SystemConstants.UserPermission);
            HttpContext.Session.Remove(SystemConstants.ModuleMenuSession);
            HttpContext.Session.Remove(SystemConstants.FunctionSession);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginVm request)
        {
            var result = _userAccountRepository.Authenticate(request);
            if (result.Success)
            {
                //logout xong mới login
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var userVm = new UserLoginResultVm();
                userVm = result.Data;

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["TokenExpires"])),
                    IsPersistent = false
                };
                var userLoggedIn = new LoggedInUser();
                userLoggedIn.UserID = userVm.UserID;
                userLoggedIn.UserName = userVm.UserName;
                userLoggedIn.UserGroup = userVm.UserGroup;
                userLoggedIn.Role = userVm.Role;
                userLoggedIn.FAdm = userVm.FAdm;

                //session login
                HttpContext.Session.SetString(SystemConstants.UserSession, JsonConvert.SerializeObject(userLoggedIn));
                HttpContext.Session.SetString(SystemConstants.AccountLogin, userLoggedIn.UserID);
                
                //session login
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userVm.UserID),
                    new Claim("UserLogin", JsonConvert.SerializeObject(userLoggedIn))
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                //var namePage = new PageDefault();
                //return RedirectToAction("Index");
                return Ok(new ApiSuccessResult<UserLoginResultVm>(userVm));
            }
            else
            {
                //ViewBag.error = result.Message;
                return Ok(new ApiErrorResult<UserLoginResultVm>(result.Message));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public dynamic GetFunctionInfo(int FunctionID)
        {
            var funcS = HttpContext.Session.GetString(SystemConstants.FunctionSession);
            var funcL = new List<FunctionMenuVm>();
            if (funcS != null)
            {
                funcL = JsonConvert.DeserializeObject<List<FunctionMenuVm>>(funcS);
            }
            var func = funcL.Find(x => x.FunctionID == FunctionID);
            return Ok(func != null ? new ApiSuccessResult<FunctionMenuVm>(func) : new ApiErrorResult<FunctionMenuVm>());
        }
    }
}