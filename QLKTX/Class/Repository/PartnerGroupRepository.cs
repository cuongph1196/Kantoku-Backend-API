using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IPartnerGroupRepository
    {
        dynamic SearchAll(string searchParams);
        dynamic GetById(int id);
        dynamic Create(PartnerGroupVm vm);
        dynamic Update(PartnerGroupVm vm);
        dynamic Delete(int id);
    }
    public class PartnerGroupRepository : IPartnerGroupRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public PartnerGroupRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic SearchAll(string searchParams)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"SearchParams", string.IsNullOrEmpty(searchParams) ? null : searchParams.Trim()}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_PartnerGroupSearch", "SearchAll");
            return result;
        }

        public dynamic GetById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerGroupKey", id}
            };
            var result = new StoredProcedureFactory<PartnerGroupVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_PartnerGroupSearch", "GetByID");
            return result;
        }

        public dynamic Create(PartnerGroupVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerGroupCode", vm.PartnerGroupCode},
                {"PartnerGroupName", vm.PartnerGroupName},
                {"Description", vm.Description},
                {"PartnerGroupParentKey", vm.PartnerGroupParentKey},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_PartnerGroup", "Create");
            return result;
        }

        public dynamic Update(PartnerGroupVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerGroupKey", vm.PartnerGroupKey},
                {"PartnerGroupCode", vm.PartnerGroupCode},
                {"PartnerGroupName", vm.PartnerGroupName},
                {"Description", vm.Description},
                {"PartnerGroupParentKey", vm.PartnerGroupParentKey},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_PartnerGroup", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerGroupKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_PartnerGroup", "Delete");
            return result;
        }
    }
}
