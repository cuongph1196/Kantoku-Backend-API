using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;

namespace QLKTX.Controllers
{
    [Route("DeptLocationExpires")]
    [ApiController]
    [Authorize]
    public class DeptLocationExpiresController : BaseController
    {
        private readonly ILogger<DeptLocationExpiresController> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDeptLocationExpiresRepository _departmentLocationRepository;
        private readonly IBuildingRepository _buildingRepository;
        public static readonly string[] imageType = { "jpg", "jpeg", "png", "gif" };

        public DeptLocationExpiresController(ILogger<DeptLocationExpiresController> logger,
            IDepartmentRepository departmentRepository,
            IDeptLocationExpiresRepository departmentLocationRepository,
            IBuildingRepository buildingRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _departmentLocationRepository = departmentLocationRepository;
            _buildingRepository = buildingRepository;
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
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;

            return View(objInfo);
        }

        [Route("GetLocation")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetLocation([FromQuery] DepartmentLocationSearchVm vm)
        {
            var resultData = new DepartmentLocationResultVm();
            var result = _buildingRepository.GetDetailImageByID(vm.BuildingSectionKey);
            if (result.Success)
            {
                resultData.BuildSImage = result.Data?.BuildingSectionImg;
                var resultL = _departmentLocationRepository.GetAllLocation(vm);
                if (resultL.Success)
                    resultData.DeptLocations = resultL.Data?.Items;

                return Ok(new ApiSuccessResult<DepartmentLocationResultVm>(resultData));
            }
            return Ok(new ApiErrorResult<string>());
        }

    }
}
