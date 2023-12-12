using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface ISystemVarRepository
    {
        dynamic GetById(string id);
        dynamic Update(SystemVarVm systemVarVM);

    }
    public class SystemVarRepository : ISystemVarRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public SystemVarRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetById(string id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"SystemVarID", id}
            };
            var result = new StoredProcedureFactory<SystemVarVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_SystemVarSearch", "GetByID");
            return result;
        }

        public dynamic Update(SystemVarVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"SystemVarID", vm.SystemVarID},
                {"SystemVarValue", vm.SystemVarValue}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_SystemVar", "Update");
            return result;
        }
    }
}
