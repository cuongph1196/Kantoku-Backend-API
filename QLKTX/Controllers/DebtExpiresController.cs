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

namespace QLKTX.Controllers
{
    [Route("DebtExpires")]
    [ApiController]
    [Authorize]
    public class DebtExpiresController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<DebtExpiresController> _logger;
        private readonly IDebtExpiresRepository _debtExpiresRepository;
        private IMemoryCache _cache;
        public DebtExpiresController(ILogger<DebtExpiresController> logger,
            IMemoryCache cache,
            IDebtExpiresRepository debtExpiresRepository,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _cache = cache;
            _debtExpiresRepository = debtExpiresRepository;
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
        public IActionResult SearchPaging([FromQuery] DebtExpiresSearchPagingVm viewModel)
        {
            var result = _debtExpiresRepository.SearchPaging(viewModel);
            return Ok(result);
        }
    }
}
