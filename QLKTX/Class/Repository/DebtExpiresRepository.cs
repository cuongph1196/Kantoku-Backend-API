using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IDebtExpiresRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(DebtExpiresSearchPagingVm vm);
    }

    public class DebtExpiresRepository : IDebtExpiresRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public DebtExpiresRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(DebtExpiresSearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"DateFrom", vm.DateFrom},
                {"DateTo", vm.DateTo},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"Status", vm.Status},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sp_frmDebtExpiresSearch", "SearchPaging");

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
    }
}
