using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IContractStatisticsReportRepository
    {
        List<IDictionary<string, object>> SearchReport(ReportSearchVm vm);
        dynamic SearchReportv2(ReportSearchVm vm);
    }
    public class ContractStatisticsReportRepository : IContractStatisticsReportRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ContractStatisticsReportRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public List<IDictionary<string, object>> SearchReport(ReportSearchVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                //{"FromDate", vm.FromDate},
                {"ToDate", vm.ToDate},
                {"BuildingKey", vm.BuildingKey > 0 ? vm.BuildingKey : null},
                {"BuildingSectionKey", vm.BuildingSectionKey > 0 ? vm.BuildingSectionKey : null},
                {"PartnerKey", vm.PartnerKey > 0 ? vm.PartnerKey : null},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sprpt_ContractStatisticsReport", "SearchDetail");
            return result.Data != null ? result.Data.Items.ToList().Select(x => (IDictionary<string, object>)x).ToList() : new List<IDictionary<string, object>>();
        }

        public dynamic SearchReportv2(ReportSearchVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                //{"FromDate", vm.FromDate},
                {"ToDate", vm.ToDate},
                {"BuildingKey", vm.BuildingKey > 0 ? vm.BuildingKey : null},
                {"BuildingSectionKey", vm.BuildingSectionKey > 0 ? vm.BuildingSectionKey : null},
                {"PartnerKey", vm.PartnerKey > 0 ? vm.PartnerKey : null},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sprpt_ContractStatisticsReport", "SearchDetail");
            return result;
        }
    }
}
