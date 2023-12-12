using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IApiLogRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(ApiLogSearchPagingVm vm);
        dynamic Create(ApiLogVm vm);
    }
    public class ApiLogRepository : IApiLogRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ApiLogRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(ApiLogSearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"TimeUtcFrom", vm.TimeUtcFrom},
                {"TimeUtcTo", vm.TimeUtcTo},
                {"RequestedMethod", string.IsNullOrEmpty(vm.RequestedMethod) ? null : vm.RequestedMethod.Trim()},
                {"UserID", string.IsNullOrEmpty(vm.UserID) ? null : vm.UserID.Trim()},
                {"SearchParams", string.IsNullOrEmpty(vm.SearchParams) ? null : vm.SearchParams.Trim()},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_ApiLog", "SearchApiLogPaging");

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

        public dynamic Create(ApiLogVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Host", vm.Host},
                {"RequestHeaders", vm.RequestHeaders},
                {"StatusCode", vm.StatusCode},
                {"RequestBody", vm.RequestBody},
                {"RequestedMethod", vm.RequestedMethod},
                {"UserHostAddress", vm.UserHostAddress},
                {"AbsoluteUri", vm.AbsoluteUri},
                {"RequestType", vm.RequestType},
                {"AccountLogin", vm.AccountLogin},
                {"TransactionID", vm.TransactionID},
                {"TransactionNo", vm.TransactionNo},
                {"FormID", vm.FormID}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).msgExecute(masterParams,
                    "spfrm_ApiLog", "Create");
            return result;
        }
    }
}
