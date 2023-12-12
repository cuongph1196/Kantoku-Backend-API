using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Authorization;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;
using System.Data;

namespace QLKTX.Controllers
{
    [Route("ClearsEmpDebtReport")]
    [ApiController]
    [Authorize]
    public class ClearsEmpDebtReportController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<ClearsEmpDebtReportController> _logger;
        private readonly IClearsEmpDebtReportRepository _vdrRepository;
        private IMemoryCache _cache;
        public ClearsEmpDebtReportController(ILogger<ClearsEmpDebtReportController> logger,
            IMemoryCache cache,
            IClearsEmpDebtReportRepository vdrRepository,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _cache = cache;
            _vdrRepository = vdrRepository;
            _environment = environment;
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

        [Route("SearchPaging")]
        [HttpGet]
        //[CustomModelValidate]
        [Permission(Action = "FView")]
        public IActionResult SearchPaging([FromQuery] SummaryVoucherRSearchPagingVm vm)
        {
            var result = _vdrRepository.SearchPaging(vm);
            return Ok(result);
        }

        [Route("SearchReport")]
        [HttpGet]
        //[CustomModelValidate]
        //[Permission(Action = "FView")]
        public dynamic SearchReport([FromQuery] string FromDate, string ToDate, string RenderType, int BuildingKey) //[FromQuery] SummaryVoucherRSearchVm viewModel
        {
            var viewModel = new SummaryVoucherRSearchVm
            {
                FromDate = FromDate, //"20231101"
                ToDate = ToDate, //"20231125"
                BuildingKey = BuildingKey,
                RenderType = RenderType

            };
            var dt = new DataTable();
            dt = _vdrRepository.SearchReport(viewModel);
            if (dt != null && dt.Rows.Count > 0)
            {
                string mineType = "";
                int extension = 1;
                var path = $"{_environment.WebRootPath}\\Reports\\rpt_VoucherDetailReport.rdlc";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                DataRow row = dt.Rows[0];
                if (row["TransNo"].ToString() == "")
                {
                    parameters.Add("BeginAmount", row["RemainAmount"].ToString());
                    row.Delete();
                    dt.AcceptChanges();
                }
                else
                {
                    parameters.Add("BeginAmount", "0");
                }
                LocalReport localR = new LocalReport(path);
                localR.AddDataSource("ds_VoucherDetail", dt);
                var result = localR.Execute(GetRenderType(viewModel.RenderType), extension, parameters, mineType);
                return File(result.MainStream, GetContentType(viewModel.RenderType), "BaoCaoChiTietThuChi" + GetExtension(viewModel.RenderType));
            }
            else
            {
                return null;
            }
        }

        private RenderType GetRenderType(string type)
        {
            RenderType renderType = new RenderType();
            switch (type)
            {
                case "Pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "Excel":
                    renderType = RenderType.Excel;
                    break;
                case "Word":
                    renderType = RenderType.Word;
                    break;
                default:
                    renderType = RenderType.Pdf;
                    break;
            }
            return renderType;
        }
        private string GetContentType(string type)
        {
            var contentType = "application/pdf";
            switch (type)
            {
                case "Pdf":
                    contentType = "application/pdf";
                    break;
                case "Excel":
                    contentType = "application/vnd.ms-excel";
                    break;
                case "Word":
                    contentType = "application/msword";
                    break;
            }
            return contentType;
        }

        private string GetExtension(string type)
        {
            var extension = ".pdf";
            switch (type)
            {
                case "Pdf":
                    extension = ".pdf";
                    break;
                case "Excel":
                    extension = ".xls";
                    break;
                case "Word":
                    extension = ".doc";
                    break;
            }
            return extension;
        }
    }
}
