using Microsoft.Extensions.Options;
using QLKTX.Class.Entities;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IPermissionRepository
    {
        dynamic GetFunctionPermission(int ModuleID, int GroupID);

        dynamic SaveFunctionPermission(PermissionSaveVm userPermissVM);
    }
    public class PermissionRepository : IPermissionRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public PermissionRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetFunctionPermission(int ModuleID, int GroupID)
        {
            var dictParams = new Dictionary<string, object>
            {
                {"ModuleID", ModuleID},
                {"UserGroupID", GroupID}
            };
            return new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(dictParams, "spfrm_PermissionSearch", "GetPermissionByGroup");
        }

        public dynamic SaveFunctionPermission(PermissionSaveVm userPermissVM)
        {
            var dictParams = new Dictionary<string, object> {
                { "ModuleID", userPermissVM.ModuleID },
                { "UserGroupID", userPermissVM.GroupID },
                { "JsonData", userPermissVM.JsonData }
            };
            return new StoredProcedureFactory<dynamic>(_connectionString).msgExecute(dictParams, "spfrm_Permission", "SavePermission");
        }
    }
}
