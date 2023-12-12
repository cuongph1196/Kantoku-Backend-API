using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QLKTX.Controllers
{
    [Route("Permission")]
    [ApiController]
    [Authorize]
    public class PermissionController : BaseController
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IPermissionRepository _permissionRepository;
        private IMemoryCache _cache;

        public PermissionController(ILogger<PermissionController> logger,
            IMemoryCache cache,
            IPermissionRepository permissionRepository)
        {
            _logger = logger;
            _cache = cache;
            _permissionRepository = permissionRepository;
        }

        public IActionResult Index(int ModuleID, int FunctionID)
        {
            var funcS = HttpContext.Session.GetString(SystemConstants.FunctionMenuSession + "_" + ModuleID);
            var funcMenu = new List<FunctionMenuVm>();
            if (funcS != null)
            {
                funcMenu = JsonConvert.DeserializeObject<List<FunctionMenuVm>>(funcS);
            }
            var permissionList = (List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + LoggedInUser.UserID);
            UserPermiss permissions = permissionList?.Where(p => p.FunctionID == FunctionID).FirstOrDefault();
            var objInfo = new PageInfoViewModel();
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;
            objInfo.FUPermiss = permissions;

            return View(objInfo);
        }

        [Route("GetPermission")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetPermission([FromQuery] int ModuleID, int GroupID)
        {
            var result = _permissionRepository.GetFunctionPermission(ModuleID, GroupID);
            return Ok(result);
        }

        [Route("SavePermission")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult SavePermission([FromBody] PermissionSaveVm model)
        {
            var result = _permissionRepository.SaveFunctionPermission(model);
            return Ok(result);
        }
    }
}
