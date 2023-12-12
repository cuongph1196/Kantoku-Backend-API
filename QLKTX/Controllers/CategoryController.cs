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
    [Route("Category")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private IMemoryCache _cache;

        public CategoryController(ILogger<CategoryController> logger,
            IMemoryCache cache,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _cache = cache;
            _categoryRepository = categoryRepository;
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
            var result = _categoryRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromQuery] int id)
        {
            var result = _categoryRepository.GetById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] CategoryVm model)
        {
            var result = _categoryRepository.Create(model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] CategoryVm model)
        {
            var result = _categoryRepository.Update(model);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var result = _categoryRepository.Delete(id);
            return Ok(result);
        }
    }
}
