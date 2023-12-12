using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using QLKTX.Class.Dtos;
using Newtonsoft.Json;
using QLKTX.Class;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

namespace QLKTX.Controllers
{
    [Route("Building")]
    [ApiController]
    [Authorize]
    public class BuildingController : BaseController
    {
        private readonly ILogger<BuildingController> _logger;
        private readonly IBuildingRepository _buildingRepository;
        private IMemoryCache _cache;

        public BuildingController(ILogger<BuildingController> logger,
            IMemoryCache cache,
            IBuildingRepository buildingRepository)
        {
            _logger = logger;
            _cache = cache;
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
            var permissionList = (List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + LoggedInUser.UserID);
            UserPermiss permissions = permissionList?.Where(p => p.FunctionID == FunctionID).FirstOrDefault();
            var objInfo = new PageInfoViewModel();
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
        public IActionResult SearchPaging([FromQuery] SearchPagingVm viewModel)
        {
            var result = _buildingRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _buildingRepository.GetById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] BuildingSaveVm vm)
        {
            var result = _buildingRepository.Create(vm.Building);
            if (result.Success)
            {
                var masterKey = (int) result.Data;
                foreach(var el in vm.BuildingSections)
                {
                    el.BuildingKey = masterKey;
                    var resultDetail = _buildingRepository.CreateDetail(el);
                    if (!resultDetail.Success) return Ok(new ApiErrorResult<string>());
                }
            }
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] BuildingSaveVm vm)
        {
            var result = _buildingRepository.Update(vm.Building); 
            if (result.Success)
            {
                foreach (var el in vm.BuildingSections)
                {
                    if (el.BuildingSectionKey > 0)
                    {
                        var updateDetail = _buildingRepository.UpdateDetail(el);
                        if (!updateDetail.Success) return Ok(new ApiErrorResult<int>());
                    }
                    else
                    {
                        var createDetail = _buildingRepository.CreateDetail(el);
                        if (!createDetail.Success) return Ok(new ApiErrorResult<string>());
                    }
                }
            }
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var resultDetail = _buildingRepository.DeleteDetailByMaster(id);
            if (resultDetail.Success)
            {
                var resultM = _buildingRepository.Delete(id);
                if (!resultM.Success) return Ok(new ApiErrorResult<int>());
            }
            return Ok(resultDetail);
        }

        [Route("DeleteDetail/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteDetailById([FromRoute] int id)
        {
            var result = _buildingRepository.DeleteDetail(id);
            return Ok(result);
        }
    }
}
