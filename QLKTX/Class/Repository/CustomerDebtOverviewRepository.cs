using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface ICustomerDebtOverviewRepository
    {
        dynamic GetContract(CDOSearchViewModel vm);
        dynamic GetDebt(CDOSearchViewModel vm);
        dynamic GetContractDetail(CDOSearchViewModel vm);
    }
    public class CustomerDebtOverviewRepository : ICustomerDebtOverviewRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public CustomerDebtOverviewRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public dynamic GetContract(CDOSearchViewModel vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerCode", vm.SearchParams},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_CustomerDebtOverviewSearch", "GetContract");
            return result;
        }

        public dynamic GetDebt(CDOSearchViewModel vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerCode", vm.SearchParams},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_CustomerDebtOverviewSearch", "GetDebt");
            return result;
        }

        public dynamic GetContractDetail(CDOSearchViewModel vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerCode", vm.SearchParams},
                {"Status", vm.Status},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_CustomerDebtOverviewSearch", "GetContractDetail");
            return result;
        }
    }
}
