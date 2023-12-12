using Microsoft.Extensions.Options;
using QLKTX.Class.Entities;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IUserPermissionRepository
    {
        dynamic GetPermissionByUser(string userID);
    }
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public UserPermissionRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetPermissionByUser(string userID)
        {
            var dictParams = new Dictionary<string, object>
            {
                {"AccountLogin", userID}
            };
            return new StoredProcedureFactory<UserPermiss>(_connectionString).FindAllBy(dictParams, "spfrm_PermissionSearch", "GetPermissionByUser");
        }
    }
}
