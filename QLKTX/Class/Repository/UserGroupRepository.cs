using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IUserGroupRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic Create(UserGroupVm vm);
        dynamic Update(UserGroupVm vm);
        dynamic Delete(int id);
    }
    public class UserGroupRepository: IUserGroupRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public UserGroupRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_UserGroupSearch", "SearchPaging");

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
            var result = new StoredProcedureFactory<UserGroupVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_UserGroupSearch", "GetByID");
            return result;
        }

        public dynamic Create(UserGroupVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"GroupName", vm.GroupName},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_UserGroup", "Create");
            return result;
        }

        public dynamic Update(UserGroupVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", vm.ID},
                {"GroupName", vm.GroupName},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_UserGroup", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_UserGroup", "Delete");
            return result;
        }
    }
}
