using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Repository;
using AuthorizeAttribute = QLKTX.Class.Authorization.AuthorizeAttribute;

namespace QLKTX.Controllers
{
    [Route("api/Common")]
    [ApiController]
    public class CommonController : Controller
    {
        private readonly IComboRepository _comboRepository;
        private readonly IAutocomplexRepository _autocomplexRepository;

        public CommonController(IComboRepository comboRepository,
            IAutocomplexRepository autocomplexRepository)
        {
            _comboRepository = comboRepository;
            _autocomplexRepository = autocomplexRepository;
        }

        #region "Combobox normal"

        /// <summary>
        ///     Get data combobox User Group
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/usergroup")]
        [HttpGet]
        public IActionResult FindAllUserGroup([FromQuery] int? active = 1)
        {
            var result = _comboRepository.FindAllUserGroup(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox module
        /// </summary>
        /// <returns></returns>
        [Route("combobox/module")]
        [HttpGet]
        public IActionResult FindAllModule()
        {
            var result = _comboRepository.FindAllModule();
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox useraccount
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/useraccount")]
        [HttpGet]
        public IActionResult FindAllUserAccount([FromQuery] int? active = 1)
        {
            var result = _comboRepository.FindAllUserAccount(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox building
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/building")]
        [HttpGet]
        public IActionResult FindAllBuilding([FromQuery] int? active = 1)
        {
            var result = _comboRepository.FindAllBuilding(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox building-section
        /// </summary>
        /// <param name="key"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/building-section")]
        [HttpGet]
        public IActionResult FindAllBuildingSection([FromQuery] int? key, int? active = 1)
        {
            var result = _comboRepository.FindAllBuildingSection(key, active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox Department
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/department")]
        [HttpGet]
        public IActionResult FindAllDepartment([FromQuery] int? active = 1)
        {
            var result = _comboRepository.FindAllDepartment(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox Department by building
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/department-by-building")]
        [HttpGet]
        public IActionResult FindAllDepartmentByBuilding([FromQuery] int buildKey, int? buildSectionKey, int? active = 1)
        {
            var result = _comboRepository.FindAllDepartmentByBuilding(buildKey, buildSectionKey, active);
            return Ok(result);
        }
        
        /// <summary>
        ///     Get data combobox Department by building
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/department-by-building-v2")]
        [HttpGet]
        public IActionResult FindAllDepartmentByBuilding2([FromQuery] int buildKey, int? buildSectionKey, int? active = 1)
        {
            var result = _comboRepository.FindAllDepartmentByBuilding2(buildKey, buildSectionKey, active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox RecordType
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/record-type")]
        [HttpGet]
        public IActionResult FindAllRecordType([FromQuery] int? active = 1)
        {
            var result = _comboRepository.FindAllRecordType(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox Partner
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/partner")]
        [HttpGet]
        public IActionResult FindAllPartner([FromQuery] int? buildKey, int? active = 1)
        {
            var result = _comboRepository.FindAllPartner(buildKey, active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox ContractDeclare
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/contract-declare")]
        [HttpGet]
        public IActionResult FindAllContractDeclare([FromQuery] int? buildKey, int? buildSectionKey, int? categoryKey, int? active = 1)
        {
            var result = _comboRepository.FindAllContractDeclare(buildKey, buildSectionKey, categoryKey, active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox category
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/category")]
        [HttpGet]
        public IActionResult FindAllCategory([FromQuery] int? active = 1)
        {
            var result = _comboRepository.FindAllCategory(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combobox department by contract
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combobox/department-by-contract")]
        [HttpGet]
        public IActionResult FindAllDepartmentByBuilding3([FromQuery] int buildKey, int? buildSectionKey, string validDate, int contractKey, int? active = 1)
        {
            var result = _comboRepository.FindAllDepartmentByBuilding3(buildKey, buildSectionKey, validDate, contractKey, active);
            return Ok(result);
        }

        #endregion

        #region "Combotree"
        /// <summary>
        ///     Get data combotree LevelCompanyStructure
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combotree/level-company-structure")]
        [HttpGet]
        public IActionResult FindAllLevelCompanyStructure([FromQuery] int active = 1)
        {
            var result = _comboRepository.FindAllLevelCompanyStructure(active);
            return Ok(result);
        }
        /// <summary>
        ///     Get data combotree CompanyStructure
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combotree/company-structure")]
        [HttpGet]
        public IActionResult FindAllCompanyStructure([FromQuery] int active = 1)
        {
            var result = _comboRepository.FindAllCompanyStructure(active);
            return Ok(result);
        }
        /// <summary>
        ///     Get data combotree CompanyStructure
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combotree/partner-group")]
        [HttpGet]
        public IActionResult FindAllPartnerGroup([FromQuery] int active = 1)
        {
            var result = _comboRepository.FindAllPartnerGroup(active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combotree reason
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combotree/reason")]
        [HttpGet]
        public IActionResult FindAllReason([FromQuery] string typeId, int? active = 1)
        {
            var result = _comboRepository.FindAllReason(typeId, active);
            return Ok(result);
        }

        /// <summary>
        ///     Get data combotree reason by transId
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [Route("combotree/reason-by-transId")]
        [HttpGet]
        public IActionResult FindAllReasonByTransID([FromQuery] string transId, int? active = 1)
        {
            var result = _comboRepository.FindAllReasonByTransID(transId, active);
            return Ok(result);
        }
        #endregion

        #region "Autocomplete"
        /// <summary>
        ///     Get data autocomplex Partner
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="buildingKey"></param>
        /// <returns></returns>
        [Route("autocomplex/partner")]
        [HttpGet]
        public IActionResult FindAutocomplexPartner(int buildingKey, string prefix)
        {
            var result = _autocomplexRepository.FindAutocomplexPartner(buildingKey, prefix);
            return Ok(result);
        }

        #endregion
    }
}
