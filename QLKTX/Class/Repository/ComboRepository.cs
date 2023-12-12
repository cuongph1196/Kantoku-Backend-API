using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IComboRepository
    {
        //combobox
        dynamic FindAllUserGroup(int? active);
        dynamic FindAllModule();
        dynamic FindAllUserAccount(int? active);
        dynamic FindAllBuilding(int? active);
        dynamic FindAllBuildingSection(int? key, int? active);
        dynamic FindAllDepartment(int? active);
        dynamic FindAllDepartmentByBuilding(int buildKey, int? buildSectionKey, int? active);
        dynamic FindAllDepartmentByBuilding2(int buildKey, int? buildSectionKey, int? active);
        dynamic FindAllRecordType(int? active);
        dynamic FindAllPartner(int? buildKey, int? active);
        dynamic FindAllContractDeclare(int? buildKey, int? buildSectionKey, int? categoryKey, int? active);
        dynamic FindAllCategory(int? active);

        //combotree
        dynamic FindAllLevelCompanyStructure(int? active);
        dynamic FindAllCompanyStructure(int? active);
        dynamic FindAllPartnerGroup(int? active);
        dynamic FindAllReason(string typeId, int? active);
        dynamic FindAllReasonByTransID(string transID, int? active);
        dynamic FindAllDepartmentByBuilding3(int buildKey, int? buildSectionKey, string validDate, int contractKey, int? active);
    }
    public class ComboRepository : IComboRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ComboRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        #region Combobox
        public dynamic FindAllUserGroup(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboGroupUser");
        }
        public dynamic FindAllModule()
        {
            var dictParams = new Dictionary<string, object>();
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboModule");
        }
        public dynamic FindAllUserAccount(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboUserAccount");
        }

        public dynamic FindAllBuilding(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboBuilding");
        }

        public dynamic FindAllBuildingSection(int? key, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("ID", key);
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboBuildingSection");
        }

        public dynamic FindAllDepartment(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboDepartment");
        }

        public dynamic FindAllDepartmentByBuilding(int buildKey, int? buildSectionKey, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            dictParams.Add("BuildingKey", buildKey);
            dictParams.Add("BuildingSectionKey", buildSectionKey);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboDepartmentByBuilding");
        }
        
        public dynamic FindAllDepartmentByBuilding2(int buildKey, int? buildSectionKey, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            dictParams.Add("BuildingKey", buildKey);
            dictParams.Add("BuildingSectionKey", buildSectionKey);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboDepartmentByBuilding2");
        }

        public dynamic FindAllRecordType(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboRecordType");
        }

        public dynamic FindAllPartner(int? buildKey, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("BuildingKey", buildKey);
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboPartner");
        }

        public dynamic FindAllContractDeclare(int? buildKey, int? buildSectionKey, int? categoryKey, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("BuildingKey", buildKey);
            dictParams.Add("BuildingSectionKey", buildSectionKey);
            dictParams.Add("CategoryKey", categoryKey);
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboContractDeclare");
        }

        public dynamic FindAllCategory(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboCategory");
        }

        public dynamic FindAllDepartmentByBuilding3(int buildKey, int? buildSectionKey, string validDate, int contractKey, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            dictParams.Add("BuildingKey", buildKey);
            dictParams.Add("BuildingSectionKey", buildSectionKey);
            dictParams.Add("ValidDate", string.IsNullOrEmpty(validDate) ? null : validDate);
            dictParams.Add("ContractKey", contractKey);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataCombobox", "GetComboDepartmentByBuilding3");
        }
        #endregion

        #region Combotree

        public dynamic FindAllLevelCompanyStructure(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<CombotreeVm>(_connectionString).FindCombotreeBy(dictParams, "sp_clsGetDataCombotree", "GetComboLevelCompanyStructure");
        }

        public dynamic FindAllCompanyStructure(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<CombotreeVm>(_connectionString).FindCombotreeBy(dictParams, "sp_clsGetDataCombotree", "GetComboCompanyStructure");
        }

        public dynamic FindAllPartnerGroup(int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<CombotreeVm>(_connectionString).FindCombotreeBy(dictParams, "sp_clsGetDataCombotree", "GetComboPartnerGroup");
        }

        public dynamic FindAllReason(string typeId, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            dictParams.Add("RecordTypeID", typeId);
            return new StoredProcedureFactory<CombotreeVm>(_connectionString).FindCombotreeBy(dictParams, "sp_clsGetDataCombotree", "GetComboReason");
        }

        public dynamic FindAllReasonByTransID(string transID, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("Active", active);
            dictParams.Add("TransID", transID);
            return new StoredProcedureFactory<CombotreeVm>(_connectionString).FindCombotreeBy(dictParams, "sp_clsGetDataCombotree", "GetComboReasonByTransID");
        }
        #endregion
    }
}
