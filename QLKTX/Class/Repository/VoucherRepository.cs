using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using QLKTX.Class.Entities;
using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IVoucherRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(VoucherSearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetDetailById(int id);
        dynamic GetCreateByContract(string rowKeyLst);
        dynamic GetCheckRelated(int id);
        dynamic GetPrintVoucher(int id);
        dynamic Create(Voucher vm);
        dynamic Update(Voucher vm);
        dynamic Delete(DeletedVm vm);
        dynamic DeleteByOldRowKey(DeletedVm vm);
        dynamic Approve(ApprovedVm vm);
        dynamic CreateDetail(VoucherDetail vm);
        dynamic UpdateDetail(VoucherDetail vm);
        dynamic DeleteDetail(int id);
        dynamic DeleteDetailByMaster(int id);
        dynamic UpdateVoucherRelated(int masterRowKey, int relatedRowKey);
        dynamic CreateVoucherDetailRelated(int masterRowKey, int relatedRowKey);
    }
    public class VoucherRepository : IVoucherRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public VoucherRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(VoucherSearchPagingVm vm)
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
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"PartnerKey", vm.PartnerKey},
                {"FunctionID", vm.FunctionID},
                {"TransID", vm.TransID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_VoucherSearch", "SearchPaging");

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
            var resultData = new VoucherSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", id},
                {"MasterRowkey", id},
            };
            var resultM = new StoredProcedureFactory<Voucher>(_connectionString)
                .FindOneBy(masterParams, "spfrm_VoucherSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.Voucher = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<VoucherDetail>(_connectionString)
                    .FindAllBy(masterParams, "spfrm_VoucherSearch", "GetDetailByMaster");
                if (resultD.Success)
                    resultData.VoucherDetails = new List<VoucherDetail>(resultD.Data.Items);
                return new ApiSuccessResult<VoucherSaveVm>(resultData);
            }
            return new ApiErrorResult<VoucherSaveVm>();
        }

        public dynamic GetDetailById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            //Get detail
            var resultD = new StoredProcedureFactory<VoucherDetail>(_connectionString)
                .FindAllBy(masterParams, "spfrm_VoucherSearch", "GetDetailByMaster");
            return resultD;
        }

        public dynamic GetCreateByContract(string rowKeyLst)
        {
            var resultData = new VoucherSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"RowKeyLst", rowKeyLst}
            };
            var resultM = new StoredProcedureFactory<Voucher>(_connectionString)
                .FindOneBy(masterParams, "spfrm_VoucherSearch", "GetCreateByContract");
            if (resultM.Success)
            {
                resultData.Voucher = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<VoucherDetail>(_connectionString)
                    .FindAllBy(masterParams, "spfrm_VoucherSearch", "GetCreateByContractDetail");
                if (resultD.Success)
                    resultData.VoucherDetails = new List<VoucherDetail>(resultD.Data.Items);
                return new ApiSuccessResult<VoucherSaveVm>(resultData);
            }
            return new ApiErrorResult<VoucherSaveVm>();
        }

        public dynamic GetCheckRelated(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            var resultM = new StoredProcedureFactory<dynamic>(_connectionString)
                .FindOneBy(masterParams, "spfrm_VoucherSearch", "GetCheckRelated");
            return resultM;
        }

        public dynamic Create(Voucher vm)
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
                //{"DepartmentKey", vm.DepartmentKey},
                {"Description", vm.Description},
                {"OldTransNo", vm.OldTransNo},
                {"OldTransID", vm.OldTransID},
                {"OldRowkey", vm.OldRowkey},
                {"Status", vm.Status}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Voucher", "Create");
            return result;
        }

        public dynamic Update(Voucher vm)
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
                //{"DepartmentKey", vm.DepartmentKey},
                {"Description", vm.Description}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Voucher", "Update");
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
                "spfrm_Voucher", "Delete");
            return result;
        }

        public dynamic DeleteByOldRowKey(DeletedVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", vm.DeletedKey},
                {"TransID", vm.TransID},
                {"IsDeleted", vm.DeletedValue}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Voucher", "DeleteByOldRowKey");
            return result;
        }
        
        public dynamic Approve(ApprovedVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", vm.ApprovedKey},
                {"Status", vm.ApprovedValue},
                {"TransID", vm.TransID}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Voucher", "Approve");
            return result;
        }

        public dynamic CreateDetail(VoucherDetail vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", vm.MasterRowkey},
                {"ReasonKey", vm.ReasonKey},
                {"InAmount", vm.InAmount},
                {"OutAmount", vm.OutAmount},
                {"ContractKey", vm.ContractKey},
                {"ContractDetailKey", vm.ContractDetailKey},
                {"DepartmentKey", vm.DepartmentKey},
                {"Description", vm.Description}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "spfrm_Voucher", "CreateDetail");
            return result;
        }

        public dynamic UpdateDetail(VoucherDetail vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DetailRowkey", vm.DetailRowkey},
                {"MasterRowkey", vm.MasterRowkey},
                {"ReasonKey", vm.ReasonKey},
                {"InAmount", vm.InAmount},
                {"OutAmount", vm.OutAmount},
                {"Description", vm.Description},
                {"DepartmentKey", vm.DepartmentKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_Voucher", "UpdateDetail");
            return result;
        }

        public dynamic DeleteDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DetailRowkey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Voucher", "DeleteDetail");
            return result;
        }

        public dynamic DeleteDetailByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Voucher", "DeleteDetailByMaster");
            return result;
        }

        //other
        public dynamic UpdateVoucherRelated(int masterRowKey, int relatedRowKey)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", masterRowKey},
                {"RelatedRowkey", relatedRowKey},
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Voucher", "UpdateVoucherRelated");
            return result;
        }
        public dynamic CreateVoucherDetailRelated(int masterRowKey, int relatedRowKey)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", masterRowKey},
                {"RelatedRowkey", relatedRowKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_Voucher", "CreateVoucherDetailRelated");
            return result;
        }

        //print
        public dynamic GetPrintVoucher(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", id}
            };
            var resultM = new StoredProcedureFactory<PrintVoucherVm>(_connectionString)
                .FindOneBy(masterParams, "spfrm_PrintVoucherSearch", "GetByID");
            return resultM;
        }
    }
}
