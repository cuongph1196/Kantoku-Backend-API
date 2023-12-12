using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface ICategoryRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic Create(CategoryVm vm);
        dynamic Update(CategoryVm vm);
        dynamic Delete(int id);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public CategoryRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_CategorySearch", "SearchPaging");

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
                {"CategoryKey", id}
            };
            var result = new StoredProcedureFactory<CategoryVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_CategorySearch", "GetByID");
            return result;
        }

        public dynamic Create(CategoryVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CategoryCode", vm.CategoryCode},
                {"CategoryName", vm.CategoryName},
                {"Description", vm.Description},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "spfrm_Category", "Create");
            return result;
        }

        public dynamic Update(CategoryVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CategoryKey", vm.CategoryKey},
                {"CategoryCode", vm.CategoryCode},
                {"CategoryName", vm.CategoryName},
                {"Description", vm.Description},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Category", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CategoryKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Category", "Delete");
            return result;
        }
    }
}
