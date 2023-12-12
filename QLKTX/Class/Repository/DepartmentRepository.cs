using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IDepartmentRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(DepartmentSearchPagingVm vm);
        dynamic GetById(int id);
        dynamic Create(DepartmentVm vm);
        dynamic Update(DepartmentVm vm);
        dynamic Delete(int id);
    }
    public class DepartmentRepository: IDepartmentRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public DepartmentRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(DepartmentSearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"SearchParams", string.IsNullOrEmpty(vm.SearchParams) ? null : vm.SearchParams.Trim()},
                {"BuildingKey", vm.BuildingKey > 0 ? vm.BuildingKey : null},
                {"BuildingSectionKey", vm.BuildingSectionKey > 0 ? vm.BuildingSectionKey : null},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_DepartmentSearch", "SearchPaging");

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
                {"DepartmentKey", id}
            };
            var result = new StoredProcedureFactory<DepartmentVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_DepartmentSearch", "GetByID");
            return result;
        }

        public dynamic Create(DepartmentVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DepartmentCode", vm.DepartmentCode},
                {"DepartmentName", vm.DepartmentName},
                {"Description", vm.Description},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"Price", vm.Price},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "spfrm_Department", "Create");
            return result;
        }

        public dynamic Update(DepartmentVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DepartmentKey", vm.DepartmentKey},
                {"DepartmentCode", vm.DepartmentCode},
                {"DepartmentName", vm.DepartmentName},
                {"Description", vm.Description},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"Price", vm.Price},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Department", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DepartmentKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Department", "Delete");
            return result;
        }
    }
}
