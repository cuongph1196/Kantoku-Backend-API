using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Models;
using System.Reflection;

namespace QLKTX.Controllers
{
    [Route("Partner")]
    [ApiController]
    [Authorize]
    public class PartnerController : BaseController
    {
        private readonly ILogger<PartnerController> _logger;
        private readonly IPartnerRepository _partnerRepository;
        private IMemoryCache _cache;

        public PartnerController(ILogger<PartnerController> logger,
            IMemoryCache cache,
            IPartnerRepository partnerRepository)
        {
            _logger = logger;
            _cache = cache;
            _partnerRepository = partnerRepository;
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
        public IActionResult SearchPaging([FromQuery] PartnerSearchPagingVm viewModel)
        {
            var result = _partnerRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _partnerRepository.GetById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] PartnerVm model)
        {
            var result = _partnerRepository.Create(model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] PartnerVm model)
        {
            var result = _partnerRepository.Update(model);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var result = _partnerRepository.Delete(id);
            return Ok(result);
        }
    }
}
