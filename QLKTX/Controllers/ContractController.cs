using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using System.Globalization;
using QLKTX.Class.Service;
using Newtonsoft.Json;
using QLKTX.Class;
using Microsoft.Extensions.Caching.Memory;

namespace QLKTX.Controllers
{
    [Route("Contract")]
    [ApiController]
    [Authorize]
    public class ContractController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<ContractController> _logger;
        private readonly IContractRepository _contractRepository;
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IUploadFileService _uploadFileService;
        private IMemoryCache _cache;
        private readonly string _transID = "CONTRACT";
        public ContractController(ILogger<ContractController> logger,
            IMemoryCache cache,
            IContractRepository contractRepository,
            IWebHostEnvironment environment,
            IFileUploadRepository fileUploadRepository,
            IUploadFileService uploadFileService)
        {
            _logger = logger;
            _cache = cache;
            _contractRepository = contractRepository;
            _environment = environment;
            _fileUploadRepository = fileUploadRepository;
            _uploadFileService = uploadFileService;
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

        [Route("ContractEntryNew")]
        [HttpGet]
        public IActionResult ContractEntryNew(int ModuleID, int FunctionID)
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

        [Route("ContractView")]
        [HttpGet]
        public IActionResult ContractView(int ModuleID, int FunctionID, int ContractKey)
        {
            var objInfo = new ContractVViewModel();
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.ContractKey = ContractKey;
            objInfo.AccountLogin = LoggedInUser.UserID;
            return View(objInfo);
        }

        [Route("ContractEdit")]
        [HttpGet]
        public IActionResult ContractEdit(int ModuleID, int FunctionID, int ContractKey)
        {
            var objInfo = new ContractVViewModel();
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.ContractKey = ContractKey;
            objInfo.AccountLogin = LoggedInUser.UserID;
            return View(objInfo);
        }

        [Route("GetDocumentsUpload")]
        [HttpGet]
        public IActionResult GetDocumentsUpload(int masterkey, int? detaiKey)
        {
            return ViewComponent("UploadDocuments", new { transID = _transID, masterkey, detaiKey, isDel = false });
        }

        [Route("SearchPaging")]
        [HttpGet]
        //[CustomModelValidate]
        [Permission(Action = "FView")]
        public IActionResult SearchPaging([FromQuery] SearchPagingVm viewModel)
        {
            var result = _contractRepository.SearchPaging(viewModel);
            return Ok(result);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _contractRepository.GetById(id);
            return Ok(result);
        }

        [Route("GetContractView/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetContractView([FromRoute] int id)
        {
            var result = _contractRepository.GetContractView(id);
            return Ok(result);
        }
        
        [Route("GetBindingContractView/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetBindingContractView([FromRoute] int id)
        {
            var resultF = new ContractViewVm();
            var result = _contractRepository.GetBindingContractView(id);
            if (result.Success)
            {
                var objData = (BindingContractViewVm)result.Data;
                var strTemplate = objData.SampleContract;
                if (strTemplate != null)
                {
                    strTemplate = strTemplate.Replace("{{soHD}}", objData.Status ? objData.ContractCode : "HĐ Nháp");
                    strTemplate = strTemplate.Replace("{{ngayHD}}", objData.ValidDate);
                    strTemplate = strTemplate.Replace("{{tenKH}}", objData.PartnerName);
                    strTemplate = strTemplate.Replace("{{CCCD}}", objData.IdentityID);
                    strTemplate = strTemplate.Replace("{{capTai}}", objData.IdentityPlaceIssue);
                    strTemplate = strTemplate.Replace("{{ngayCap}}", objData.IdentityDateIssue);
                    strTemplate = strTemplate.Replace("{{diaChiKH}}", objData.PartnerAddress);
                    strTemplate = strTemplate.Replace("{{mstKH}}", objData.PartnerTaxNumber);
                    strTemplate = strTemplate.Replace("{{sdtKH}}", objData.PartnerPhone);
                    strTemplate = strTemplate.Replace("{{tenCho}}", objData.BuildingName);
                    strTemplate = strTemplate.Replace("{{diaChiCho}}", objData.BuildingAddress);
                    strTemplate = strTemplate.Replace("{{dienTichSap}}", "");
                    strTemplate = strTemplate.Replace("{{ThoiHanThue}}", objData.ContractTermByMonth.ToString());
                    strTemplate = strTemplate.Replace("{{ThueTuNgay}}", objData.ValidDate);
                    strTemplate = strTemplate.Replace("{{ThueDenNgay}}", DateTime.ParseExact(objData.ValidDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddMonths(objData.ContractTermByMonth).ToString("dd/MM/yyyy"));
                    strTemplate = strTemplate.Replace("{{NgayBanGiao}}", "");
                    strTemplate = strTemplate.Replace("{{NgayTinhTien}}", "");

                    resultF.ContractKey = objData.ContractKey;
                    resultF.ContractView = strTemplate;
                    return Ok(new ApiSuccessResult<ContractViewVm>(resultF));
                }
                else
                {
                    return Ok(new ApiErrorResult<string>("Chưa khai báo template HĐ mẫu"));
                }
            }
            return Ok(new ApiErrorResult<ContractViewVm>(resultF));
        }

        [Route("GetDataCalculator")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult GetDataCalculator([FromBody] ContractSaveVm vm)
        {
            var result = _contractRepository.GetDataCalculator(vm);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] ContractSaveVm vm)
        {
            var result = _contractRepository.Create(vm.Contract);
            if (result.Success)
            {
                var masterKey = (int)result.Data;
                foreach (var el in vm.CTemps)
                {
                    el.ContractKey = masterKey;
                    var resultDetail = _contractRepository.CreateContractTemp(el);
                    if (!resultDetail.Success) return Ok(new ApiErrorResult<string>());
                }
                foreach (var el in vm.CDetails)
                {
                    el.ContractKey = masterKey;
                    var resultDetail = _contractRepository.CreateDetail(el);
                    if (!resultDetail.Success) return Ok(new ApiErrorResult<string>());
                }
            }
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] ContractSaveVm vm)
        {
            var result = _contractRepository.Update(vm.Contract);
            if (result.Success)
            {
                //delete contract temp
                if (vm.CTemps != null && vm.CTemps.Count > 0 && (vm.CTemps[0].ContractTempKey == null || vm.CTemps[0].ContractTempKey == 0))
                {
                    _contractRepository.DeleteContractTempByMaster((int) vm.Contract.ContractKey);
                }
                foreach (var el in vm.CTemps)
                {
                    el.ContractKey = vm.Contract.ContractKey;
                    if (el.ContractTempKey > 0)
                    {
                        var updateDetail = _contractRepository.UpdateContractTemp(el);
                        if (!updateDetail.Success) return Ok(new ApiErrorResult<int>());
                    }
                    else
                    {
                        var createDetail = _contractRepository.CreateContractTemp(el);
                        if (!createDetail.Success) return Ok(new ApiErrorResult<string>());
                    }
                }
                foreach (var el in vm.CDetails)
                {
                    el.ContractKey = vm.Contract.ContractKey;
                    if (el.ContractDetailKey > 0)
                    {
                        var updateDetail = _contractRepository.UpdateDetail(el);
                        if (!updateDetail.Success) return Ok(new ApiErrorResult<int>());
                    }
                    else
                    {
                        var createDetail = _contractRepository.CreateDetail(el);
                        if (!createDetail.Success) return Ok(new ApiErrorResult<string>());
                    }
                }
            }
            return Ok(result);
        }
        
        [Route("Approve/{key}")]
        [HttpPut]
        [Permission(Action = "FApp")]
        //[CustomModelValidate]
        public IActionResult Approve([FromRoute] int key)
        {
            var obj = new ContractAppVm()
            {
                ContractKey = key,
                Status = true
            };
            var result = _contractRepository.Approve(obj);
            return Ok(result);
        }

        [Route("Reject/{key}")]
        [HttpPut]
        [Permission(Action = "FReject")]
        //[CustomModelValidate]
        public IActionResult Reject([FromRoute] int key)
        {
            var obj = new ContractAppVm()
            {
                ContractKey = key,
                Status = false
            };
            var result = _contractRepository.Approve(obj);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            var resultDetail = _contractRepository.DeleteDetailByMaster(id);
            if (resultDetail.Success)
            {
                var resultDocs = _fileUploadRepository.DeleteByMasterKey(id, _transID);
                //if (!resultDocs.Success) return Ok(new ApiErrorResult<int>());

                var resultM = _contractRepository.Delete(id);
                if (!resultM.Success) return Ok(new ApiErrorResult<int>());
            }
            return Ok(resultDetail);
        }

        [Route("DeleteDetailByMaster/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteDetailByMaster([FromRoute] int id)
        {
            var resultDetail = _contractRepository.DeleteDetailByMaster(id);
            return Ok(resultDetail);
        }

        [Route("DeleteDetail/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteDetailById([FromRoute] int id)
        {
            var result = _contractRepository.DeleteDetail(id);
            return Ok(result);
        }

        [Route("UpdateContractView")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult UpdateContractView([FromBody] ContractViewVm vm)
        {
            var result = _contractRepository.UpdateContractView(vm);
            return Ok(result);
        }

        [Route("UploadFiles")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFiles()
        {
            var result = new ApiResult<int>();
            var currentRequest = HttpContext.Request;
            var hostFull = currentRequest.Scheme + "://" + currentRequest.Host.Value;
            if (currentRequest.Form.Files.Count > 0)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\Upload"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                }
                var dict = currentRequest.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                var key = string.Empty;
                foreach (var item in dict)
                {
                    if (item.Key == "MasterKey")
                    {
                        key = item.Value;
                        continue;
                    }
                }
                if (key != string.Empty)
                {
                    // Loop through uploaded files  
                    for (var i = 0; i < currentRequest.Form.Files.Count; i++)
                    {
                        IFormFile vmFile = currentRequest.Form.Files[i];
                        var fileType = Path.GetExtension(vmFile.FileName.ToLowerInvariant()).Substring(1);//get file type
                        var rUrl = await _uploadFileService.UploadFiles(vmFile);
                        if (rUrl != null)
                        {
                            var objF = new FileUploadVm()
                            {
                                TransID = _transID,
                                MasterKey = int.Parse(key),
                                DetailKey = null,
                                FileName = vmFile.FileName,
                                FileType = fileType,
                                FileUrl = hostFull + rUrl
                            };
                            result = _fileUploadRepository.Create(objF);
                            if (!result.Success) return Ok(new ApiErrorResult<int>(-1));
                        }
                        else
                        {
                            return Ok(new ApiErrorResult<string>("thêm file thất bại !!!"));
                        }
                    }
                    return Ok(result);
                }
                return Ok(new ApiErrorResult<string>());
            }
            return Ok(new ApiErrorResult<string>("Không có files"));
        }


        [Route("DeleteFileUpload/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteFileUpload([FromRoute] int id)
        {
            var result = _fileUploadRepository.Delete(id);
            return Ok(result);
        }

    }
}
