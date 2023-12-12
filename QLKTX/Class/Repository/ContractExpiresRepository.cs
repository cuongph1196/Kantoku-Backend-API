using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IContractExpiresRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(ContractExpiresSearchPagingVm vm);
    }
    public class ContractExpiresRepository : IContractExpiresRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ContractExpiresRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(ContractExpiresSearchPagingVm vm)
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
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sp_frmContractExpiresSearch", "SearchPaging");

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
