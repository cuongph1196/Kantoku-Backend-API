using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using Newtonsoft.Json;
using QLKTX.Class;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Entities;

namespace QLKTX.Controllers
{
    [Route("CompanyStructure")]
    [ApiController]
    [Authorize]
    public class CompanyStructureController : BaseController
    {
        private readonly ILogger<CompanyStructureController> _logger;
        private readonly ICompanyStructureRepository _companyStructureRepository;
        private IMemoryCache _cache;

        public CompanyStructureController(ILogger<CompanyStructureController> logger,
            IMemoryCache cache,
            ICompanyStructureRepository companyStructureRepository)
        {
            _logger = logger;
            _cache = cache;
            _companyStructureRepository = companyStructureRepository;
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

        [Route("SearchAll")]
        [HttpGet]
        //[CustomModelValidate]
        [Permission(Action = "FView")]
        public IActionResult SearchAll([FromQuery] string SearchParams)
        {
            var result = _companyStructureRepository.SearchAll(SearchParams);
            return Ok(result);
        }

        [Route("GetById")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromQuery] int id)
        {
            var result = _companyStructureRepository.GetById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] CompanyStructureVm model)
        {
            var result = _companyStructureRepository.Create(model);
            if (result.Success)
            {
                _companyStructureRepository.CreateCompanyStructureFull();
            }
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] CompanyStructureVm model)
        {
            var result = _companyStructureRepository.Update(model);
            if (result.Success)
            {
                _companyStructureRepository.CreateCompanyStructureFull();
            }
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var result = _companyStructureRepository.Delete(id);
            if (result.Success)
            {
                _companyStructureRepository.CreateCompanyStructureFull();
            }
            return Ok(result);
        }
    }
}
