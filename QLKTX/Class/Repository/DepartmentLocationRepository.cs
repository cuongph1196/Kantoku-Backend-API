using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IDepartmentLocationRepository
    {
        dynamic GetAllLocation(DepartmentLocationSearchVm vm);
        dynamic GetByID(int id);
        dynamic Create(DepartmentLocationVm vm);
        dynamic Update(DepartmentLocationVm vm);
        dynamic Delete(int id);
        dynamic DeleteByBuild(DepartmentLocationDelVm vm);
    }
    public class DepartmentLocationRepository : IDepartmentLocationRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public DepartmentLocationRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_DepartmentLocationSearch", "GetAllLocation");
            return result;
        }

        public dynamic GetByID(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                { "ID", id}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindOneBy(masterParams,
                "spfrm_DepartmentLocationSearch", "GetByID");
            return result;
        }

        public dynamic Create(DepartmentLocationVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"xAxis", vm.xAxis},
                {"yAxis", vm.yAxis},
                {"DepartmentKey", vm.DepartmentKey},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_DepartmentLocation", "Create");
            return result;
        }

        public dynamic Update(DepartmentLocationVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", vm.ID},
                {"xAxis", vm.xAxis},
                {"yAxis", vm.yAxis},
                {"DepartmentKey", vm.DepartmentKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_DepartmentLocation", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ID", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_DepartmentLocation", "Delete");
            return result;
        }

        public dynamic DeleteByBuild(DepartmentLocationDelVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_DepartmentLocation", "DeleteByBuild");
            return result;
        }
    }
}
