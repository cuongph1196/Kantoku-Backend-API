using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;

namespace QLKTX.Controllers.Mobile
{
    [Route("api/PosDeviceM")]
    [ApiController]
    [AuthorizeAttributeM]
    public class PosDeviceMController : BaseMController
    {
        private readonly ILogger<PosDeviceMController> _logger;
        private readonly IPosDeviceRepository _posDeviceRepository;
        private IMemoryCache _cache;

        public PosDeviceMController(ILogger<PosDeviceMController> logger,
            IMemoryCache cache,
            IPosDeviceRepository posDeviceRepository)
        {
            _logger = logger;
            _cache = cache;
            _posDeviceRepository = posDeviceRepository;
        }

        [AllowAnonymous]
        [Route("Create")]
        [HttpPost]
        //[PermissionM(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] PosDeviceVm model)
        {
            var result = _posDeviceRepository.Create(model);
            if (result.Success)
            {
                return Ok(new ApiSuccessResult<string>("Thêm mới thành công !!!"));
            }
            else
            {
                if(result.Data == -2)
                {
                    return Ok(new ApiErrorResult<string>("Mã thiết bị đã tồn tại !!!"));
                }
                else
                {
                    return Ok(new ApiErrorResult<string>("Xảy ra lỗi trong quá trình thực thi !!!"));
                }
            }
        }

        [Route("CheckDevice/{deviceCode}")]
        [HttpGet]
        //[Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult CheckDevice([FromRoute] string deviceCode)
        {
            var result = _posDeviceRepository.CheckDevice(deviceCode);
            return Ok(result);
        }

    }
}
