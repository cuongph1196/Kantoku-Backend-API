using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IPartnerRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(PartnerSearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetByBuilding(PartnerSearchAPIVm vm);
        dynamic Create(PartnerVm vm);
        dynamic Update(PartnerVm vm);
        dynamic Delete(int id);
    }
    public class PartnerRepository : IPartnerRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;
        private readonly int _functionID = 20;

        public PartnerRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(PartnerSearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"SearchParams", string.IsNullOrEmpty(vm.SearchParams) ? null : vm.SearchParams.Trim()},
                {"PartnerGroupkey", vm.PartnerGroupkey > 0 ? vm.PartnerGroupkey : null},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_PartnerSearch", "SearchPaging");

            var data = result.Success ? new List<dynamic>(result.Data.Items) : new List<dynamic>();
            var totalRow = data.Any() ? data.Select(x => x.TotalRow).FirstOrDefault() : 0;

            var dataResult = new DataTablePagingResultVm<dynamic>
            {
                data = data,
                draw = vm.Draw,
                recordsFiltered = totalRow,
                recordsTotal = totalRow
            };

            return dataResult;
        }

        public dynamic GetById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerKey", id}
            };
            var result = new StoredProcedureFactory<PartnerVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_PartnerSearch", "GetByID");
            return result;
        }

        public dynamic GetByBuilding(PartnerSearchAPIVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerKey", vm.BuildingKey},
                {"Active", vm.Active},
                {"FunctionID", _functionID}
            };
            var result = new StoredProcedureFactory<PartnerVm>(_connectionString).FindAllBy(masterParams,
                "spfrm_PartnerSearch", "GetByBuildingAPI");
            return result;
        }

        public dynamic Create(PartnerVm vm)
        {
            var partnerNonUnicodeSearch = string.Concat(
                "/", StringExtension.ConvertToUnSignName(vm.PartnerCode).ToLowerInvariant(),
                "/", StringExtension.ConvertToUnSignName(vm.PartnerName).ToLowerInvariant(),
                "/", StringExtension.ConvertToUnSignName(vm.PartnerTaxNumber).ToLowerInvariant(),
                "/", StringExtension.ConvertToUnSignName(vm.PartnerPhone).ToLowerInvariant(), "/");
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerCode", vm.PartnerCode},
                {"PartnerName", vm.PartnerName},
                {"PartnerNonUnicodeSearch", partnerNonUnicodeSearch},
                {"IdentityID", vm.IdentityID},
                {
                    "IdentityDateIssue", string.IsNullOrEmpty(vm.IdentityDateIssue)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.IdentityDateIssue)
                },
                {"IdentityPlaceIssue", vm.IdentityPlaceIssue},
                {"PartnerAddress", vm.PartnerAddress},
                {"PartnerPhone", vm.PartnerPhone},
                {"PartnerTaxNumber", vm.PartnerTaxNumber},
                {"PartnerTaxName", vm.PartnerTaxName},
                {"PartnerTaxAddress", vm.PartnerTaxAddress},
                {"PartnerGroupkey", vm.PartnerGroupkey},
                {"IsEmployee", vm.IsEmployee},
                {"IsCustomer", vm.IsCustomer},
                {"IsVendor", vm.IsVendor},
                {"NFCCode", vm.NFCCode},
                {"BuildingKey", vm.BuildingKey},
                {"Description", vm.Description},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_Partner", "Create");
            return result;
        }

        public dynamic Update(PartnerVm vm)
        {
            var partnerNonUnicodeSearch = string.Concat(
                "/", StringExtension.ConvertToUnSignName(vm.PartnerCode).ToLowerInvariant(),
                "/", StringExtension.ConvertToUnSignName(vm.PartnerName).ToLowerInvariant(),
                "/", StringExtension.ConvertToUnSignName(vm.PartnerTaxNumber).ToLowerInvariant(),
                "/", StringExtension.ConvertToUnSignName(vm.PartnerPhone).ToLowerInvariant(), "/");
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerKey", vm.PartnerKey},
                {"PartnerCode", vm.PartnerCode},
                {"PartnerName", vm.PartnerName},
                {"PartnerNonUnicodeSearch", partnerNonUnicodeSearch},
                {"IdentityID", vm.IdentityID},
                {
                    "IdentityDateIssue", string.IsNullOrEmpty(vm.IdentityDateIssue)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.IdentityDateIssue)
                },
                {"IdentityPlaceIssue", vm.IdentityPlaceIssue},
                {"PartnerAddress", vm.PartnerAddress},
                {"PartnerPhone", vm.PartnerPhone},
                {"PartnerTaxNumber", vm.PartnerTaxNumber},
                {"PartnerTaxName", vm.PartnerTaxName},
                {"PartnerTaxAddress", vm.PartnerTaxAddress},
                {"PartnerGroupkey", vm.PartnerGroupkey},
                {"IsEmployee", vm.IsEmployee},
                {"IsCustomer", vm.IsCustomer},
                {"IsVendor", vm.IsVendor},
                {"NFCCode", vm.NFCCode},
                {"BuildingKey", vm.BuildingKey},
                {"Description", vm.Description},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Partner", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PartnerKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Partner", "Delete");
            return result;
        }
    }
}
