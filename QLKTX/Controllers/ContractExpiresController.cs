using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.Service;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Models;

namespace QLKTX.Controllers
{
    [Route("ContractExpires")]
    [ApiController]
    [Authorize]
    public class ContractExpiresController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<ContractExpiresController> _logger;
        private readonly IContractExpiresRepository _contractRepository;
        private IMemoryCache _cache;
        public ContractExpiresController(ILogger<ContractExpiresController> logger,
            IMemoryCache cache,
            IContractExpiresRepository contractRepository,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _cache = cache;
            _contractRepository = contractRepository;
            _environment = environment;
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


        [Route("SearchPaging")]
        [HttpGet]
        //[CustomModelValidate]
        [Permission(Action = "FView")]
        public IActionResult SearchPaging([FromQuery] ContractExpiresSearchPagingVm viewModel)
        {
            var result = _contractRepository.SearchPaging(viewModel);
            return Ok(result);
        }

    }
}
