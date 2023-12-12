using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Class.Authorization;
using QLKTX.Class.Filters;
using QLKTX.Models;
using Newtonsoft.Json;
using QLKTX.Class;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Entities;

namespace QLKTX.Controllers
{
    [Route("UserGroup")]
    [ApiController]
    [Authorize]
    public class UserGroupController : BaseController
    {
        private readonly ILogger<UserGroupController> _logger;
        private readonly IUserGroupRepository _userGroupRepository;
        private IMemoryCache _cache;

        public UserGroupController(ILogger<UserGroupController> logger,
            IMemoryCache cache,
            IUserGroupRepository userGroupRepository)
        {
            _logger = logger;
            _cache = cache;
            _userGroupRepository = userGroupRepository;
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
        public IActionResult SearchPaging([FromQuery] SearchPagingVm viewModel)
        {
            var result = _userGroupRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromQuery] int id)
        {
            var result = _userGroupRepository.GetById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create( UserGroupVm model)
        {
            var result = _userGroupRepository.Create(model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] UserGroupVm model)
        {
            var result = _userGroupRepository.Update(model);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var result = _userGroupRepository.Delete(id);
            return Ok(result);
        }
    }
}
