using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class.Entities;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Class;
using QLKTX.Models;
using QLKTX.Class.Authorization;
using QLKTX.Class.Filters;

namespace QLKTX.Controllers
{
    [Route("UserInfo")]
    [ApiController]
    //[Authorize]
    public class UserInfoController : BaseController
    {
        private readonly ILogger<UserInfoController> _logger;
        private readonly IUserAccountRepository _userAccountRepository;
        private IMemoryCache _cache;

        public UserInfoController(ILogger<UserInfoController> logger,
            IMemoryCache cache,
            IUserAccountRepository userAccountRepository)
        {
            _logger = logger;
            _cache = cache;
            _userAccountRepository = userAccountRepository;
        }

        public IActionResult Index()
        {
            var objInfo = new PageInfoViewModel();
            objInfo.AccountLogin = LoggedInUser.UserID;

            return View(objInfo);
        }


        [Route("GetById")]
        [HttpGet]
        public IActionResult GetById()
        {
            var result = _userAccountRepository.GetById(LoggedInUser.UserID);
            return Ok(result);
        }

        [Route("UpdateName")]
        [HttpPut]
        //[CustomModelValidate]
        public IActionResult UpdateName([FromBody] UserAccountVm model)
        {
            var result = _userAccountRepository.UpdateName(model);
            return Ok(result);
        }

        [Route("UpdatePass")]
        [HttpPut]
        [CustomModelValidate]
        public IActionResult UpdatePass([FromBody] ChangePassVm model)
        {
            var result = _userAccountRepository.UpdatePass(model);
            return Ok(result);
        }
    }
}
