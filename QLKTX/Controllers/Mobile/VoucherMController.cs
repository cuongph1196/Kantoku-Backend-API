using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Authorization;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.Service;

namespace QLKTX.Controllers.Mobile
{
    [Route("api/VoucherM")]
    [ApiController]
    [AuthorizeAttributeM]
    public class VoucherMController : BaseMController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<VoucherMController> _logger;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IAutoTransNoRepository _autoTransNoRepository;
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IUploadFileService _uploadFileService;
        private readonly ISystemVarRepository _systemVarRepository;
        private IMemoryCache _cache;

        public VoucherMController(ILogger<VoucherMController> logger,
            IMemoryCache cache,
            IVoucherRepository voucherRepository,
            IAutoTransNoRepository autoTransNoRepository,
            IWebHostEnvironment environment,
            IFileUploadRepository fileUploadRepository,
            IUploadFileService uploadFileService,
            ISystemVarRepository systemVarRepository)
        {
            _logger = logger;
            _cache = cache;
            _voucherRepository = voucherRepository;
            _autoTransNoRepository = autoTransNoRepository;
            _environment = environment;
            _fileUploadRepository = fileUploadRepository;
            _uploadFileService = uploadFileService;
            _systemVarRepository = systemVarRepository;
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _voucherRepository.GetById(id);
            return Ok(result);
        }
    }
}
