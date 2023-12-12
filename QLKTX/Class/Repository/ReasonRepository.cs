using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IReasonRepository
    {
        dynamic SearchAll(string searchParams);
        dynamic GetById(int id);
        dynamic Create(ReasonVm vm);
        dynamic Update(ReasonVm vm);
        dynamic Delete(int id);
    }
    public class ReasonRepository: IReasonRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ReasonRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_ReasonSearch", "SearchAll");
            return result;
        }

        public dynamic GetById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ReasonKey", id}
            };
            var result = new StoredProcedureFactory<ReasonVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_ReasonSearch", "GetByID");
            return result;
        }

        public dynamic Create(ReasonVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ReasonCode", vm.ReasonCode},
                {"ReasonName", vm.ReasonName},
                {"Description", vm.Description},
                {"RecordID", vm.RecordID},
                {"ReasonParentID", vm.ReasonParentID},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_Reason", "Create");
            return result;
        }

        public dynamic Update(ReasonVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ReasonKey", vm.ReasonKey},
                {"ReasonCode", vm.ReasonCode},
                {"ReasonName", vm.ReasonName},
                {"Description", vm.Description},
                {"RecordID", vm.RecordID},
                {"ReasonParentID", vm.ReasonParentID},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Reason", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ReasonKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Reason", "Delete");
            return result;
        }
    }
}
