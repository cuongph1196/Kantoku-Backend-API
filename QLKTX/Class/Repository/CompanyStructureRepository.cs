using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface ICompanyStructureRepository
    {
        dynamic SearchAll(string searchParams);
        dynamic GetById(int id);
        dynamic Create(CompanyStructureVm vm);
        dynamic Update(CompanyStructureVm vm);
        dynamic Delete(int id);
        void CreateCompanyStructureFull();
    }
    public class CompanyStructureRepository : ICompanyStructureRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public CompanyStructureRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_CompanyStructureSearch", "SearchAll");
            return result;
        }

        public dynamic GetById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CompanyStructureKey", id}
            };
            var result = new StoredProcedureFactory<CompanyStructureVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_CompanyStructureSearch", "GetByID");
            return result;
        }

        public dynamic Create(CompanyStructureVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CompanyStructureCode", vm.CompanyStructureCode},
                {"CompanyStructureName", vm.CompanyStructureName},
                {"CompanyStructureParent", vm.CompanyStructureParent},
                {"Description", vm.Description},
                {"LevelCompanyStructureKey", vm.LevelCompanyStructureKey},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_CompanyStructure", "Create");
            return result;
        }

        public dynamic Update(CompanyStructureVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CompanyStructureKey", vm.CompanyStructureKey},
                {"CompanyStructureCode", vm.CompanyStructureCode},
                {"CompanyStructureName", vm.CompanyStructureName},
                {"CompanyStructureParent", vm.CompanyStructureParent},
                {"Description", vm.Description},
                {"LevelCompanyStructureKey", vm.LevelCompanyStructureKey},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_CompanyStructure", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"CompanyStructureKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_CompanyStructure", "Delete");
            return result;
        }

        public void CreateCompanyStructureFull()
        {
            var masterParams = new Dictionary<string, object>();
            new StoredProcedureFactory<string>(_connectionString).voidExecute(masterParams,
                "spfrm_CompanyStructure", "CreateCompanyStructureFull");
        }
    }
}
