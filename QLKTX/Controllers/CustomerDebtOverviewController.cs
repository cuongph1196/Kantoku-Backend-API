using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Dtos;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Helper;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;

namespace QLKTX.Controllers
{
    [Route("CustomerDebtOverview")]
    [ApiController]
    [Authorize]
    public class CustomerDebtOverviewController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<CustomerDebtOverviewController> _logger;
        private readonly ICustomerDebtOverviewRepository _customerDebtOverviewRepository;
        private IMemoryCache _cache;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IDebtRepository _debtRepository;
        private readonly IAutoTransNoRepository _autoTransNoRepository;
        private readonly ISystemVarRepository _systemVarRepository;
        public CustomerDebtOverviewController(ILogger<CustomerDebtOverviewController> logger,
            IMemoryCache cache,
            ICustomerDebtOverviewRepository customerDebtOverviewRepository,
            IWebHostEnvironment environment,
            IVoucherRepository voucherRepository,
            IDebtRepository debtRepository,
            IAutoTransNoRepository autoTransNoRepository,
            ISystemVarRepository systemVarRepository)
        {
            _logger = logger;
            _cache = cache;
            _customerDebtOverviewRepository = customerDebtOverviewRepository;
            _environment = environment;
            _voucherRepository = voucherRepository;
            _debtRepository = debtRepository;
            _autoTransNoRepository = autoTransNoRepository;
            _systemVarRepository = systemVarRepository;
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

        [Route("GetContract")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetContract([FromQuery] CDOSearchViewModel viewModel)
        {
            var result = _customerDebtOverviewRepository.GetContract(viewModel);
            return Ok(result);
        }

        [Route("GetDebt")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetDebt([FromQuery] CDOSearchViewModel viewModel)
        {
            var result = _customerDebtOverviewRepository.GetDebt(viewModel);
            return Ok(result);
        }

        [Route("GetContractDetail")]
        [HttpGet]
        [Permission(Action = "FView")]
        public IActionResult GetContractDetail([FromQuery] CDOSearchViewModel viewModel)
        {
            var result = _customerDebtOverviewRepository.GetContractDetail(viewModel);
            return Ok(result);
        }

        [Route("CreateVoucher")]
        [HttpPost]
        [Permission(Action = "FAdd")]
        public IActionResult CreateVoucher([FromBody] CreateVoucherFromCDOVm vm)
        {
            //var flag = false;
            //foreach (int rowKey in vm.RowKeys)
            //{
            //    var result = this.CreateVoucherOther(rowKey);
            //    if (result.Success) 
            //        flag = true;
            //    else
            //        return Ok(new ApiErrorResult<string>("Tạo phiếu chi thất bại !"));
            //}

            //return Ok(flag ? new ApiSuccessResult<int>() : new ApiErrorResult<int>());
            var newTransID = "THU01";
            var dataM = _voucherRepository.GetCreateByContract(vm.RowKeyLst);
            if (dataM.Success)
            {
                VoucherSaveVm vData = dataM.Data;
                //Auto Create TransactionNo
                var transNo = _autoTransNoRepository.GetAutoTransNo(newTransID,
                    StringFormatDateHelper.ConvertDate(DateTime.Now.ToString("yyyyMMdd")));
                if ("N/A".Equals(transNo)) return Ok(new ApiErrorResult<int>());
                vData.Voucher.TransID = newTransID;
                vData.Voucher.Rowkey = 0;
                vData.Voucher.TransNo = transNo;
                vData.Voucher.Status = true;
                var resultCM = _voucherRepository.Create(vData.Voucher);
                if (resultCM.Success)
                {
                    foreach (var item in vData.VoucherDetails)
                    {
                        item.MasterRowkey = resultCM.Data;
                        item.DetailRowkey = 0;
                        var resultCD = _voucherRepository.CreateDetail(item);
                        if (!resultCD.Success) return Ok(new ApiErrorResult<int>());
                    }
                }
                return Ok(resultCM);
            }
            else
                return Ok(new ApiErrorResult<int>());
        }

        private dynamic CreateVoucherOther(int masterRowKey)
        {
            var newTransID = "THU01";
            var dataM = _debtRepository.GetCreateVoucherById(masterRowKey);
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
    }
}
