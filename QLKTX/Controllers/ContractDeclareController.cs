using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using Newtonsoft.Json;
using QLKTX.Class;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Entities;

namespace QLKTX.Controllers
{
    [Route("ContractDeclare")]
    [ApiController]
    [Authorize]
    public class ContractDeclareController : BaseController
    {
        private readonly ILogger<ContractDeclareController> _logger;
        private readonly IContractDeclareRepository _contractDeclareRepository;
        private IMemoryCache _cache;

        public ContractDeclareController(ILogger<ContractDeclareController> logger,
            IMemoryCache cache,
            IContractDeclareRepository contractDeclareRepository)
        {
            _logger = logger;
            _cache = cache;
            _contractDeclareRepository = contractDeclareRepository;
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

        [Route("SampleContract")]
        [HttpGet]
        public IActionResult SampleContract(int ModuleID, int FunctionID, int ContractDeclareKey)
        {
            var permissionList = (List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + LoggedInUser.UserID);
            UserPermiss permissions = permissionList?.Where(p => p.FunctionID == FunctionID).FirstOrDefault();
            var objInfo = new SampleContractViewModel();
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.ContractDeclareKey = ContractDeclareKey;
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
            var result = _contractDeclareRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _contractDeclareRepository.GetById(id);
            return Ok(result);
        }
        
        [Route("GetSampleContract/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetSampleContract([FromRoute] int id)
        {
            var result = _contractDeclareRepository.GetSampleContract(id);
            return Ok(result);
        }

        [Route("GetDetailById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetDetailById([FromRoute] int id)
        {
            var result = _contractDeclareRepository.GetDetailById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] ContractDeclareSaveVm vm)
        {
            var result = _contractDeclareRepository.Create(vm.ContractDeclare);
            if (result.Success)
            {
                var masterKey = (int)result.Data;
                foreach (var el in vm.CDDetails)
                {
                    el.ContractDeclareKey = masterKey;
                    var resultDetail = _contractDeclareRepository.CreateDetail(el);
                    if (!resultDetail.Success) return Ok(new ApiErrorResult<string>());
                }
            }
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] ContractDeclareSaveVm vm)
        {
            var result = _contractDeclareRepository.Update(vm.ContractDeclare);
            if (result.Success)
            {
                foreach (var el in vm.CDDetails)
                {
                    if (el.ContractDeclareDetailKey > 0)
                    {
                        var updateDetail = _contractDeclareRepository.UpdateDetail(el);
                        if (!updateDetail.Success) return Ok(new ApiErrorResult<int>());
                    }
                    else
                    {
                        var createDetail = _contractDeclareRepository.CreateDetail(el);
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
            var resultDetail = _contractDeclareRepository.DeleteDetailByMaster(id);
            if (resultDetail.Success)
            {
                var resultM = _contractDeclareRepository.Delete(id);
                if (!resultM.Success) return Ok(new ApiErrorResult<int>());
            }
            return Ok(resultDetail);
        }

        [Route("DeleteDetail/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteDetailById([FromRoute] int id)
        {
            var result = _contractDeclareRepository.DeleteDetail(id);
            return Ok(result);
        }

        [Route("UpdateSampleContract")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult UpdateSampleContract([FromBody] SampleContractVm vm)
        {
            var result = _contractDeclareRepository.UpdateSampleContract(vm);
            return Ok(result);
        }

    }
}
