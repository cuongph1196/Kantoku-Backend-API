using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IDeptLocationExpiresRepository
    {
        dynamic GetAllLocation(DepartmentLocationSearchVm vm);
    }
    public class DeptLocationExpiresRepository : IDeptLocationExpiresRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public DeptLocationExpiresRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetAllLocation(DepartmentLocationSearchVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                { "BuildingKey", vm.BuildingKey},
                { "BuildingSectionKey", vm.BuildingSectionKey}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_DeptLocationExpires", "GetAllLocation");
            return result;
        }
    }
}
