using QLKTX.Class.Entities;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IPosVoucherRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(VoucherSearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetDetailById(int id);
        dynamic Create(PosVoucher vm);
        dynamic Update(PosVoucher vm);
        dynamic Delete(DeletedVm vm);
        dynamic Approve(ApprovedVm vm);
        dynamic CreateDetail(PosVoucherDetail vm);
        dynamic UpdateDetail(PosVoucherDetail vm);
        dynamic DeleteDetail(int id);
        dynamic DeleteDetailByMaster(int id);
    }
    public class PosVoucherRepository : IPosVoucherRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public PosVoucherRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                {"FunctionID", vm.FunctionID},
                {"TransID", vm.TransID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "spfrm_POSVoucherSearch", "SearchPaging");

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
            var resultData = new PosVoucherSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"Rowkey", id},
                {"MasterRowkey", id},
            };
            var resultM = new StoredProcedureFactory<PosVoucher>(_connectionString)
                .FindOneBy(masterParams, "spfrm_POSVoucherSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.Voucher = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<PosVoucherDetail>(_connectionString)
                    .FindAllBy(masterParams, "spfrm_POSVoucherSearch", "GetDetailByMaster");
                if (resultD.Success)
                    resultData.VoucherDetails = new List<PosVoucherDetail>(resultD.Data.Items);
                return new ApiSuccessResult<PosVoucherSaveVm>(resultData);
            }
            return new ApiErrorResult<PosVoucherSaveVm>();
        }

        public dynamic GetDetailById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            //Get detail
            var resultD = new StoredProcedureFactory<PosVoucherDetail>(_connectionString)
                .FindAllBy(masterParams, "spfrm_POSVoucherSearch", "GetDetailByMaster");
            return resultD;
        }

        public dynamic Create(PosVoucher vm)
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
                {"Description", vm.Description}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_POSVoucher", "Create");
            return result;
        }

        public dynamic Update(PosVoucher vm)
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
                    "spfrm_POSVoucher", "Update");
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
                "spfrm_POSVoucher", "Delete");
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
                "spfrm_POSVoucher", "Approve");
            return result;
        }

        public dynamic CreateDetail(PosVoucherDetail vm)
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
                    "spfrm_POSVoucher", "CreateDetail");
            return result;
        }

        public dynamic UpdateDetail(PosVoucherDetail vm)
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
                    "spfrm_POSVoucher", "UpdateDetail");
            return result;
        }

        public dynamic DeleteDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DetailRowkey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_POSVoucher", "DeleteDetail");
            return result;
        }

        public dynamic DeleteDetailByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"MasterRowkey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_POSVoucher", "DeleteDetailByMaster");
            return result;
        }
    }
}
