using QLKTX.Class.Entities;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IDebtRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(DebtSearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetCreateVoucherById(int id);
        dynamic GetDetailById(int id);
        dynamic GetDebtByContract(DebtCreateByContractSearchVm vm);
        dynamic Create(Debt vm);
        dynamic Update(Debt vm);
        dynamic Delete(DeletedVm vm);
        dynamic Approve(ApprovedVm vm);
        dynamic CreateDetail(DebtDetail vm);
        dynamic UpdateDetail(DebtDetail vm);
        dynamic DeleteDetail(int id);
        dynamic DeleteDetailByMaster(int id);
        dynamic UpdateContractDetail(int id);
    }
    public class DebtRepository : IDebtRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public DebtRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(DebtSearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"DateFrom", vm.DateFrom},
                {"DateTo", vm.DateTo},
                {"SearchParams", string.IsNullOrEmpty(vm.SearchParams) ? null : vm.SearchParams.Trim()},
                {"FunctionID", vm.FunctionID},
                {"TransID", vm.TransID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_DebtSearch", "SearchPaging");

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
            var resultData = new DebtSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", id},
                {"MasterRowkey", id},
            };
            var resultM = new StoredProcedureFactory<Debt>(_connectionString)
                .FindOneBy(masterParams, "spfrm_DebtSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.Debt = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<DebtDetail>(_connectionString)
                    .FindAllBy(masterParams, "spfrm_DebtSearch", "GetDetailByMaster");
                if (resultD.Success)
                    resultData.DebtDetails = new List<DebtDetail>(resultD.Data.Items);
                return new ApiSuccessResult<DebtSaveVm>(resultData);
            }
            return new ApiErrorResult<DebtSaveVm>();
        }

        public dynamic GetDetailById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            //Get detail
            var resultD = new StoredProcedureFactory<DebtDetail>(_connectionString)
                .FindAllBy(masterParams, "spfrm_DebtSearch", "GetDetailByMaster");
            return resultD;
        }

        public dynamic GetDebtByContract(DebtCreateByContractSearchVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DateFrom", vm.DateFrom},
                {"DateTo", vm.DateTo},
            };
            //Get detail
            var resultD = new StoredProcedureFactory<DebtCreateByContractVm>(_connectionString)
                .FindAllBy(masterParams, "spfrm_DebtSearch", "GetDebtByContract");
            return resultD;
        }

        public dynamic GetCreateVoucherById(int id)
        {
            var resultData = new VoucherSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", id},
                {"MasterRowkey", id},
            };
            var resultM = new StoredProcedureFactory<Voucher>(_connectionString)
                .FindOneBy(masterParams, "spfrm_DebtSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.Voucher = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<VoucherDetail>(_connectionString)
                    .FindAllBy(masterParams, "spfrm_DebtSearch", "GetDetailByMaster");
                if (resultD.Success)
                    resultData.VoucherDetails = new List<VoucherDetail>(resultD.Data.Items);
                return new ApiSuccessResult<VoucherSaveVm>(resultData);
            }
            return new ApiErrorResult<VoucherSaveVm>();
        }

        public dynamic Create(Debt vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"TransNo", vm.TransNo},
                {"TransID", vm.TransID},
                {
                    "TransDate", string.IsNullOrEmpty(vm.TransDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.TransDate)
                },
                {"PartnerKey", vm.PartnerKey},
                {"PartnerTaxNumber", vm.PartnerTaxNumber},
                {"PartnerTaxName", vm.PartnerTaxName},
                {"PartnerTaxAddress", vm.PartnerTaxAddress},
                {"TaxTemplateNo", vm.TaxTemplateNo},
                {"TaxSerialNo", vm.TaxSerialNo},
                {"TaxNo", vm.TaxNo},
                {
                    "TaxDate", string.IsNullOrEmpty(vm.TaxDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.TaxDate)
                },
                {"BuildingKey", vm.BuildingKey},
                {"DepartmentKey", vm.DepartmentKey},
                {"Description", vm.Description},
                {"ContractKey", vm.ContractKey},
                {"ContractDetailKey", vm.ContractDetailKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Debt", "Create");
            return result;
        }

        public dynamic Update(Debt vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", vm.Rowkey},
                {"TransNo", vm.TransNo},
                {"TransID", vm.TransID},
                {
                    "TransDate", string.IsNullOrEmpty(vm.TransDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.TransDate)
                },
                {"PartnerKey", vm.PartnerKey},
                {"PartnerTaxNumber", vm.PartnerTaxNumber},
                {"PartnerTaxName", vm.PartnerTaxName},
                {"PartnerTaxAddress", vm.PartnerTaxAddress},
                {"TaxTemplateNo", vm.TaxTemplateNo},
                {"TaxSerialNo", vm.TaxSerialNo},
                {"TaxNo", vm.TaxNo},
                {
                    "TaxDate", string.IsNullOrEmpty(vm.TaxDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.TaxDate)
                },
                {"BuildingKey", vm.BuildingKey},
                {"DepartmentKey", vm.DepartmentKey},
                {"Description", vm.Description}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Debt", "Update");
            return result;
        }

        public dynamic Delete(DeletedVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", vm.DeletedKey},
                {"IsDeleted", vm.DeletedValue}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Debt", "Delete");
            return result;
        }

        public dynamic Approve(ApprovedVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", vm.ApprovedKey},
                {"Status", vm.ApprovedValue}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Debt", "Approve");
            return result;
        }

        public dynamic CreateDetail(DebtDetail vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", vm.MasterRowkey},
                {"ReasonKey", vm.ReasonKey},
                {"InAmount", vm.InAmount},
                {"OutAmount", vm.OutAmount},
                {"Description", vm.Description}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "spfrm_Debt", "CreateDetail");
            return result;
        }

        public dynamic UpdateDetail(DebtDetail vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DetailRowkey", vm.DetailRowkey},
                {"MasterRowkey", vm.MasterRowkey},
                {"ReasonKey", vm.ReasonKey},
                {"InAmount", vm.InAmount},
                {"OutAmount", vm.OutAmount},
                {"Description", vm.Description}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Debt", "UpdateDetail");
            return result;
        }

        public dynamic DeleteDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DetailRowkey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Debt", "DeleteDetail");
            return result;
        }

        public dynamic DeleteDetailByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Debt", "DeleteDetailByMaster");
            return result;
        }

        public dynamic UpdateContractDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDetailKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Debt", "UpdateContractDetail");
            return result;
        }
    }
}
