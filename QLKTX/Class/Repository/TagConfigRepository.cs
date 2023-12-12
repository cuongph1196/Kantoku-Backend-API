using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Class.Entities;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface ITagConfigRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic Create(TagConfig vm);
        dynamic Update(TagConfig vm);
        dynamic Delete(int id);
    }
    public class TagConfigRepository : ITagConfigRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public TagConfigRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_TagConfigSearch", "SearchPaging");

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
                {"DeviceKey", id}
            };
            var result = new StoredProcedureFactory<TagConfig>(_connectionString).FindOneBy(masterParams,
                "spfrm_TagConfigSearch", "GetByID");
            return result;
        }

        public dynamic Create(TagConfig vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"TagConfigCode", vm.TagConfigCode},
                {"TagConfigName", vm.TagConfigName},
                {"Description", vm.Description},
                {"ColorCode", vm.ColorCode},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "spfrm_TagConfig", "Create");
            return result;
        }

        public dynamic Update(TagConfig vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"TagConfigKey", vm.TagConfigKey},
                {"TagConfigCode", vm.TagConfigCode},
                {"TagConfigName", vm.TagConfigName},
                {"Description", vm.Description},
                {"ColorCode", vm.ColorCode},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_TagConfig", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"TagConfigKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_TagConfig", "Delete");
            return result;
        }
    }
}
