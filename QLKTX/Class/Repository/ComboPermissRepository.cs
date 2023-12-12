using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IComboPermissRepository
    {
        dynamic FindAllBuildingPermiss(int functionId, int? active);
    }
    public class ComboPermissRepository : IComboPermissRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ComboPermissRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        #region combobox
        public dynamic FindAllBuildingPermiss(int functionId, int? active)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("FunctionID", functionId);
            dictParams.Add("Active", active);
            return new StoredProcedureFactory<ComboboxVm>(_connectionString).FindComboboxBy(dictParams, "sp_clsGetDataComboboxByPermiss", "GetComboBuildingPermiss");
        }

        #endregion
    }
}
