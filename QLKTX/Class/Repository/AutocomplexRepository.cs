using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;

namespace QLKTX.Class.Repository
{
    public interface IAutocomplexRepository
    {
        dynamic FindAutocomplexPartner(int buildingKey, string prefix);
    }
    public class AutocomplexRepository : IAutocomplexRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public AutocomplexRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic FindAutocomplexPartner(int buildingKey, string prefix)
        {
            var dictParams = new Dictionary<string, object>();
            dictParams.Add("SearchParam", StringExtension.convertToUnSign3(prefix.Trim()));
            dictParams.Add("BuildingKey", buildingKey > 0 ? buildingKey : null);
            return new StoredProcedureFactory<AutocomplexVm>(_connectionString).FindAutocomplexBy(dictParams, "sp_clsGetAutoComplete", "GetAutocomplexPartner");
        }
    }
}
