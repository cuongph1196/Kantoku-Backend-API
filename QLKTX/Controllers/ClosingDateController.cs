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

namespace QLKTX.Controllers
{
    [Route("ClosingDate")]
    [ApiController]
    [Authorize]
    public class ClosingDateController : BaseController
    {
        private readonly ILogger<ClosingDateController> _logger;
        private readonly ISystemVarRepository _systemVarRepository;
        private IMemoryCache _cache;

        public ClosingDateController(ILogger<ClosingDateController> logger,
            IMemoryCache cache,
            ISystemVarRepository systemVarRepository)
        {
            _logger = logger;
            _cache = cache;
            _systemVarRepository = systemVarRepository;
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
            objInfo.FunctionID = FunctionID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;
            objInfo.FUPermiss = permissions;

            return View(objInfo);
        }

        [Route("GetById")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromQuery] string id)
        {
            var result = _systemVarRepository.GetById(id);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] SystemVarVm model)
        {
            var result = _systemVarRepository.Update(model);
            return Ok(result);
        }

    }
}
