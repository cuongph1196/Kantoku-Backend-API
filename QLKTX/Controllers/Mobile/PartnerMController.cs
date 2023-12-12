using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Authorization;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;

namespace QLKTX.Controllers.Mobile
{
    [Route("api/PartnerM")]
    [ApiController]
    [AuthorizeAttributeM]
    public class PartnerMController : BaseMController
    {
        private readonly ILogger<PartnerMController> _logger;
        private readonly IPartnerRepository _partnerRepository;
        private IMemoryCache _cache;

        public PartnerMController(ILogger<PartnerMController> logger,
            IMemoryCache cache,
            IPartnerRepository partnerRepository)
        {
            _logger = logger;
            _cache = cache;
            _partnerRepository = partnerRepository;
        }

        [Route("GetByBuilding")]
        [HttpGet]
        //[Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult GetByBuilding([FromQuery] PartnerSearchAPIVm vm)
        {
            var result = _partnerRepository.GetByBuilding(vm);
            return Ok(result);
        }
    }
}
