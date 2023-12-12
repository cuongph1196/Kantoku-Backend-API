using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using System.Reflection;

namespace QLKTX.Controllers
{
    [Route("ApiLog")]
    [ApiController]
    [Authorize]
    public class ApiLogController : BaseController
    {
        private readonly ILogger<ApiLogController> _logger;
        private readonly IApiLogRepository _apiLogRepository;

        public ApiLogController(ILogger<ApiLogController> logger,
            IApiLogRepository apiLogRepository)
        {
            _logger = logger;
            _apiLogRepository = apiLogRepository;
        }

        public IActionResult Index(int ModuleID, int FunctionID)
        {
            var funcS = HttpContext.Session.GetString(SystemConstants.FunctionMenuSession + "_" + ModuleID);
            var funcMenu = new List<FunctionMenuVm>();
            if (funcS != null)
            {
                funcMenu = JsonConvert.DeserializeObject<List<FunctionMenuVm>>(funcS);
            }
            var objInfo = new PageInfoViewModel();
            objInfo.FunctionID = FunctionID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;

            return View(objInfo);
        }

        [Route("SearchPaging")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult SearchPaging([FromQuery] ApiLogSearchPagingVm viewModel)
        {
            var result = _apiLogRepository.SearchPaging(viewModel);
            return Ok(result);
        }
    }
}
