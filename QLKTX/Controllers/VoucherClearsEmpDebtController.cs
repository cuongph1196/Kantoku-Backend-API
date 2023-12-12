using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using QLKTX.Class.Helper;
using QLKTX.Class.Service;
using Microsoft.Extensions.Caching.Memory;
using QLKTX.Class.Entities;
using Microsoft.AspNetCore.Http;

namespace QLKTX.Controllers
{
    [Route("VoucherClearsEmpDebt")]
    [ApiController]
    [Authorize]
    public class VoucherClearsEmpDebtController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<VoucherClearsEmpDebtController> _logger;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IAutoTransNoRepository _autoTransNoRepository;
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly IUploadFileService _uploadFileService;
        private readonly ISystemVarRepository _systemVarRepository;
        private IMemoryCache _cache;

        public VoucherClearsEmpDebtController(ILogger<VoucherClearsEmpDebtController> logger,
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

        public IActionResult Index(int ModuleID, int FunctionID, string TransID)
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
            objInfo.TransID = TransID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;
            objInfo.FUPermiss = permissions;

            return View(objInfo);
        }

        [Route("VCEDEntry")]
        [HttpGet]
        public IActionResult VCEDEntry(int ModuleID, int FunctionID, string TransID)
        {
            var funcS = HttpContext.Session.GetString(SystemConstants.FunctionMenuSession + "_" + ModuleID);
            var funcMenu = new List<FunctionMenuVm>();
            if (funcS != null)
            {
                funcMenu = JsonConvert.DeserializeObject<List<FunctionMenuVm>>(funcS);
            }
            var permissionList = (List<UserPermiss>)_cache.Get<dynamic>(SystemConstants.UserPermission + "_" + LoggedInUser.UserID);
            UserPermiss permissions = permissionList?.Where(p => p.FunctionID == FunctionID).FirstOrDefault();
            var closingDate = _systemVarRepository.GetById("ClosingDate");
            var objInfo = new PageInfoViewModel();
            objInfo.ModuleID = ModuleID;
            objInfo.FunctionID = FunctionID;
            objInfo.TransID = TransID;
            objInfo.FunctionName = funcMenu.Find(x => x.FunctionID == FunctionID)?.FunctionName;
            objInfo.AccountLogin = LoggedInUser.UserID;
            objInfo.FUPermiss = permissions;
            objInfo.ClosingDate = closingDate.Success ? closingDate.Data?.SystemVarValue : null;
            return View(objInfo);
        }

        [Route("SearchPaging")]
        [HttpGet]
        //[CustomModelValidate]
        [Permission(Action = "FView")]
        public IActionResult SearchPaging([FromQuery] VoucherSearchPagingVm vm)
        {
            var result = _voucherRepository.SearchPaging(vm);
            return Ok(result);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _voucherRepository.GetById(id);
            return Ok(result);
        }

        [Route("GetDocumentsUpload")]
        [HttpGet]
        public IActionResult GetDocumentsUpload(string transID, int masterkey, int? detaiKey)
        {
            return ViewComponent("UploadDocuments", new { transID, masterkey, detaiKey });
        }


        [Route("GetDetailById/{id}")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetDetailById([FromRoute] int id)
        {
            var result = _voucherRepository.GetDetailById(id);
            return Ok(result);
        }

        [Route("Create")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        //[CustomModelValidate]
        public IActionResult Create([FromBody] VoucherSaveVm vm)
        {
            //Auto Create TransactionNo
            var transNo = _autoTransNoRepository.GetAutoTransNo(vm.Voucher.TransID,
                StringFormatDateHelper.ConvertDate(vm.Voucher.TransDate));
            if ("N/A".Equals(transNo)) return Ok(new ApiErrorResult<int>());
            vm.Voucher.TransNo = transNo;
            var result = _voucherRepository.Create(vm.Voucher);
            if (result.Success)
            {
                var masterKey = (int)result.Data;
                foreach (var el in vm.VoucherDetails)
                {
                    el.MasterRowkey = masterKey;
                    var resultDetail = _voucherRepository.CreateDetail(el);
                    if (!resultDetail.Success) return Ok(new ApiErrorResult<string>());
                }
            }
            return Ok(result);
        }

        [Route("Update")]
        [HttpPut]
        [Permission(Action = "FEdit")]
        //[CustomModelValidate]
        public IActionResult Update([FromBody] VoucherSaveVm vm)
        {
            var result = _voucherRepository.Update(vm.Voucher);
            if (result.Success)
            {
                foreach (var el in vm.VoucherDetails)
                {
                    if (el.DetailRowkey > 0)
                    {
                        var updateDetail = _voucherRepository.UpdateDetail(el);
                        if (!updateDetail.Success) return Ok(new ApiErrorResult<int>());
                    }
                    else
                    {
                        var createDetail = _voucherRepository.CreateDetail(el);
                        if (!createDetail.Success) return Ok(new ApiErrorResult<string>());
                    }
                }
            }
            return Ok(result);
        }

        [Route("Delete")]
        [HttpPut]
        [Permission(Action = "FDel")]
        public IActionResult Delete([FromBody] DeletedVm vm)
        {
            var resultM = _voucherRepository.Delete(vm);
            return Ok(resultM);
        }

        [Route("Restore")]
        [HttpPut]
        [Permission(Action = "FDel")]
        public IActionResult Restore([FromBody] DeletedVm vm)
        {
            var resultM = _voucherRepository.Delete(vm);
            return Ok(resultM);
        }

        [Route("DeleteDetail/{id}")]
        [HttpDelete]
        [Permission(Action = "FDel")]
        public IActionResult DeleteDetailById([FromRoute] int id)
        {
            var result = _voucherRepository.DeleteDetail(id);
            return Ok(result);
        }

        [Route("Approve")]
        [HttpPut]
        [Permission(Action = "FApp")]
        public IActionResult Approve([FromBody] ApprovedVm vm)
        {
            var resultM = _voucherRepository.Approve(vm);
            if (resultM.Success)
            {
                var checkData = _voucherRepository.GetCheckRelated(vm.ApprovedKey);
                if (checkData.Success && checkData.Data?.RelatedRowKey != null)
                {
                    var resultCN = this.UpdateVoucherOther(vm.ApprovedKey, checkData.Data?.RelatedRowKey);
                    if (!resultCN.Success) return Ok(new ApiErrorResult<string>("Cập nhật phiếu chi thất bại !"));
                }
                else
                {
                    var resultCN = this.CreateVoucherOther(vm.ApprovedKey);
                    if (!resultCN.Success) return Ok(new ApiErrorResult<string>("Tạo phiếu chi thất bại !"));
                }
            }
            return Ok(resultM);
        }

        [Route("Reject")]
        [HttpPut]
        [Permission(Action = "FReject")]
        public IActionResult Reject([FromBody] ApprovedVm vm)
        {
            var resultM = _voucherRepository.Approve(vm);
            if (resultM.Success)
            {
                var objDelete = new DeletedVm()
                {
                    DeletedKey = vm.ApprovedKey,
                    TransID = vm.TransID,
                    DeletedValue = true
                };
                var resultDO = _voucherRepository.DeleteByOldRowKey(objDelete);
                if (!resultDO.Success) return Ok(new ApiErrorResult<string>("Xóa phiếu chi thất bại !"));
            }
            return Ok(resultM);
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
                var transID = string.Empty;
                foreach (var item in dict)
                {
                    if (item.Key == "MasterKey")
                    {
                        key = item.Value;
                        continue;
                    }
                    if (item.Key == "TransID")
                    {
                        transID = item.Value;
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
                                TransID = transID,
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

        //thêm mới CHI11
        private dynamic CreateVoucherOther(int masterRowKey)
        {
            var newTransID = "CHI11";
            var dataM = _voucherRepository.GetById(masterRowKey);
            if (dataM.Success)
            {
                VoucherSaveVm vData = dataM.Data;
                //Auto Create TransactionNo
                var transNo = _autoTransNoRepository.GetAutoTransNo(newTransID,
                    StringFormatDateHelper.ConvertDate(vData.Voucher.TransDate));
                if ("N/A".Equals(transNo)) return Ok(new ApiErrorResult<int>());
                vData.Voucher.OldTransID = vData.Voucher.TransID;
                vData.Voucher.TransID = newTransID;
                vData.Voucher.OldRowkey = vData.Voucher.Rowkey;
                vData.Voucher.Rowkey = 0;
                vData.Voucher.OldTransNo = vData.Voucher.TransNo;
                vData.Voucher.TransNo = transNo;
                vData.Voucher.Status = true;
                var resultCM = _voucherRepository.Create(vData.Voucher);
                if (!resultCM.Success) return new ApiErrorResult<int>();

                foreach (var item in vData.VoucherDetails)
                {
                    item.MasterRowkey = resultCM.Data;
                    item.DetailRowkey = 0;
                    var resultCD = _voucherRepository.CreateDetail(item);
                    if (!resultCD.Success) return new ApiErrorResult<int>();
                }
            }
            return dataM;
        }

        private dynamic UpdateVoucherOther(int masterRowKey, int relatedRowKey)
        {
            var result = _voucherRepository.UpdateVoucherRelated(masterRowKey, relatedRowKey);
            if (result.Success)
            {
                var delDetail = _voucherRepository.DeleteDetailByMaster(relatedRowKey);
                //if(!delDetail.Success) return new ApiErrorResult<int>();

                var resultD = _voucherRepository.CreateVoucherDetailRelated(masterRowKey, relatedRowKey);
                if (!resultD.Success) return new ApiErrorResult<int>();
            }
            return result;
        }
    }
}
