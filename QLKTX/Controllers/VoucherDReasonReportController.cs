using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using QLKTX.Class.Entities;
using QLKTX.Class.Filters;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Class;
using QLKTX.Models;
using QLKTX.Class.Authorization;
using AspNetCore.Reporting;
using System.Data;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Controllers
{
    [Route("VoucherDReasonReport")]
    [ApiController]
    [Authorize]
    public class VoucherDReasonReportController : BaseController
    {
        public static IWebHostEnvironment _environment;
        private readonly ILogger<VoucherDReasonReportController> _logger;
        private readonly IVoucherDReasonReportRepository _vdrRepository;
        private IMemoryCache _cache;
        public VoucherDReasonReportController(ILogger<VoucherDReasonReportController> logger,
            IMemoryCache cache,
            IVoucherDReasonReportRepository vdrRepository,
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

        [Route("SearchReportv2")]
        [HttpPost]
        //[CustomModelValidate]
        [Permission(Action = "FView")]
        public IActionResult SearchReportv2([FromBody] ReportSearchVm vm)
        {
            var result = _vdrRepository.SearchReportv2(vm);
            return Ok(result);
        }

        [Route("ExportExcel")]
        [HttpGet]
        public IActionResult ExportExcel([FromQuery] string FromDate, string ToDate, int? BuildingKey)
        {
            string reportname = $"ThongKeThuChiTheoKhoanMuc_{Guid.NewGuid():N}.xlsx";
            var objParams = new ReportSearchVm
            {
                FromDate = FromDate,
                ToDate = ToDate,
                BuildingKey = BuildingKey
            };
            var data = _vdrRepository.SearchReport(objParams);
            if (data.Count > 0)
            {
                var exportbytes = ExporttoExcel<dynamic>(data, reportname);
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
            }
            else
            {
                return null;
            }
        }
        private byte[] ExporttoExcel<T>(List<IDictionary<string, object>> data, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            List<string> tittle = data[0].Select(x => x.Key).ToList();
            for (int row = 0; row < tittle.Count; row++)
            {
                ws.Cells[1, row + 1].Value = tittle[row];
            }
            for (int column = 0; column < data.Count; column++)
            {
                List<string> dataColumn = data[column].Select(x => x.Value.ToString()).ToList();

                for (int row = 0; row < dataColumn.Count; row++)
                {
                    ws.Cells[column + 2, row + 1].Value = dataColumn[row].ToString();
                }
            }
            // Set auto width all column
            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            //Custom style sheet using ExcelTable
            using (ExcelRange Rng = ws.Cells[ws.Dimension.Address])
            {
                ExcelTableCollection tblcollection = ws.Tables;
                ExcelTable table = tblcollection.Add(Rng, "ThongKeThuChiTheoKhoanMuc");

                table.ShowFilter = true;
                table.TableStyle = TableStyles.Light9;
            }
            return pack.GetAsByteArray();
        }
    }
}
