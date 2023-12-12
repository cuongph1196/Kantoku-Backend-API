using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IMenuRepository
    {
        dynamic GetFunctionMenu(int? moduleID);
        dynamic GetModuleMenu();
        dynamic GetAllFunction();
    }
    public class MenuRepository : IMenuRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public MenuRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetFunctionMenu(int? moduleID)
        {
            var masterParams = new Dictionary<string, object>();
            masterParams.Add("ModuleID", moduleID);
            var result = new StoredProcedureFactory<FunctionMenuVm>(_connectionString).FindAllBy(masterParams,
                "spfrm_MenuSearch", "GetFunctionMenuByUserID");
            return result;
        }

        public dynamic GetModuleMenu()
        {
            var masterParams = new Dictionary<string, object>();
            var result = new StoredProcedureFactory<ModuleMenuVm>(_connectionString).FindAllBy(masterParams,
                "spfrm_MenuSearch", "GetModuleMenuByUserID");
            return result;
        }

        public dynamic GetAllFunction()
        {
            var masterParams = new Dictionary<string, object>();
            var result = new StoredProcedureFactory<FunctionMenuVm>(_connectionString).FindAllBy(masterParams,
                "spfrm_MenuSearch", "GetAllFunctionByUserID");
            return result;
        }
    }
}
