using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Repository;

namespace QLKTX.Controllers
{
    [Route("api/Common/Permiss")]
    [ApiController]
    public class CommonPermissController : Controller
    {
        private readonly IComboPermissRepository _comboPermissRepository;

        public CommonPermissController(IComboPermissRepository comboPermissRepository)
        {
            _comboPermissRepository = comboPermissRepository;
        }

        #region "Combobox normal"

        /// <summary>
        ///     Get data combobox building
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/building")]
        [HttpGet]
        public IActionResult FindAllBuilding([FromQuery] int functionId, int? active = 1)
        {
            var result = _comboPermissRepository.FindAllBuildingPermiss(functionId, active);
            return Ok(result);
        }
        #endregion
    }
}
