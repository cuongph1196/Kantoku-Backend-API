using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using Newtonsoft.Json;
using QLKTX.Class;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

namespace QLKTX.Controllers
{
    [Route("Department")]
    [ApiController]
    [Authorize]
    public class DepartmentController : BaseController
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private IMemoryCache _cache;

        public DepartmentController(ILogger<DepartmentController> logger,
            IMemoryCache cache,
            IDepartmentRepository departmentRepository)
        {
            _logger = logger;
            _cache = cache;
            _departmentRepository = departmentRepository;
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
        public IActionResult SearchPaging([FromQuery] DepartmentSearchPagingVm viewModel)
        {
            var result = _departmentRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromQuery] int id)
        {
            var result = _departmentRepository.GetById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] DepartmentVm model)
        {
            var result = _departmentRepository.Create(model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] DepartmentVm model)
        {
            var result = _departmentRepository.Update(model);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var result = _departmentRepository.Delete(id);
            return Ok(result);
        }
    }
}
