using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;
using System.Collections;
using System.Data.SqlClient;

namespace QLKTX.Class.Repository
{
    public interface IVoucherDReasonReportRepository
    {
        List<IDictionary<string, object>> SearchReport(ReportSearchVm vm);
        dynamic SearchReportv2(ReportSearchVm vm);
    }
    public class VoucherDReasonReportRepository : IVoucherDReasonReportRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public VoucherDReasonReportRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }
        public List<IDictionary<string, object>> SearchReport(ReportSearchVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"FromDate", vm.FromDate},
                {"ToDate", vm.ToDate},
                {"BuildingKey", vm.BuildingKey > 0 ? vm.BuildingKey : null},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sprpt_VoucherDReasonReportv2", "GetVoucherDetailReason");
            return result.Data != null ? result.Data.Items.ToList().Select(x => (IDictionary<string, object>)x).ToList() : new List<IDictionary<string, object>>();
        }

        public dynamic SearchReportv2(ReportSearchVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"FromDate", vm.FromDate},
                {"ToDate", vm.ToDate},
                {"BuildingKey", vm.BuildingKey > 0 ? vm.BuildingKey : null},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sprpt_VoucherDReasonReportv2", "GetVoucherDetailReason");
            return result;
        }
    }
}
