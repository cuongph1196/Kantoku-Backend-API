using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using System.Collections;
using System.Data.SqlClient;

namespace QLKTX.Class.Repository
{
    public interface IVoucherDetailReportRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SummaryVoucherRSearchPagingVm vm);
        dynamic SearchReport(SummaryVoucherRSearchVm vm);
    }
    public class VoucherDetailReportRepository : IVoucherDetailReportRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public VoucherDetailReportRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(SummaryVoucherRSearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"FromDate", vm.FromDate},
                {"ToDate", vm.ToDate},
                {"BuildingKey", vm.BuildingKey},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sprpt_VoucherDetailReport", "SearchPaging");

            var data = result.Success ? new List<dynamic>(result.Data.Items) : new List<dynamic>();
            var totalRow = data.Any() ? data.Select(x => x.TotalRow).FirstOrDefault() : 0;

            var dataResult = new DataTablePagingResultVm<dynamic>
            {
                data = data,
                draw = vm.Draw,
                recordsFiltered = totalRow,
                recordsTotal = totalRow
            };

            return dataResult;
        }

        public dynamic SearchReport(SummaryVoucherRSearchVm vm)
        {
            ArrayList arrParams = new ArrayList();
            arrParams.Add(new SqlParameter("@FromDate", vm.FromDate));
            arrParams.Add(new SqlParameter("@ToDate", vm.ToDate));
            arrParams.Add(new SqlParameter("@BuildingKey", vm.BuildingKey > 0 ? vm.BuildingKey : null));
            //arrParams.Add(new SqlParameter("@FunctionID", vm.FunctionID));

            var result = new StoredProcedureFactory<dynamic>(_connectionString).GetDataTable(arrParams,
                "sprpt_VoucherDetailReport", "GetVoucherDetail");
            return result;
        }
    }
}
