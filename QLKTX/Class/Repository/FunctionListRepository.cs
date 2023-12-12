using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IFunctionListRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic Create(FunctionListVm vm);
        dynamic Update(FunctionListVm vm);
        dynamic Delete(int id);
    }
    public class FunctionListRepository : IFunctionListRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public FunctionListRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"SearchParams", string.IsNullOrEmpty(vm.SearchParams) ? null : vm.SearchParams.Trim()},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_FunctionListSearch", "SearchPaging");

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

        public dynamic GetById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", id}
            };
            var result = new StoredProcedureFactory<FunctionListVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_FunctionListSearch", "GetByID");
            return result;
        }

        public dynamic Create(FunctionListVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"FunctionName", vm.FunctionName},
                {"ModuleID", vm.ModuleID},
                {"Url", vm.Url},
                {"Rank", vm.Rank},
                {"Display", vm.Display},
                {"ParentID", vm.ParentID},
                {"Icons", vm.Icons},
                {"TransID", vm.TransID},
                {"IsPopup", vm.IsPopup}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_FunctionList", "Create");
            return result;
        }

        public dynamic Update(FunctionListVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", vm.ID},
                {"FunctionName", vm.FunctionName},
                {"ModuleID", vm.ModuleID},
                {"Url", vm.Url},
                {"Rank", vm.Rank},
                {"Display", vm.Display},
                {"ParentID", vm.ParentID},
                {"Icons", vm.Icons},
                {"TransID", vm.TransID},
                {"IsPopup", vm.IsPopup}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_FunctionList", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_FunctionList", "Delete");
            return result;
        }
    }
}
