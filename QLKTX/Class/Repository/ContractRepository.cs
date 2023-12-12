using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;
using Newtonsoft.Json;
using QLKTX.Class.Entities;

namespace QLKTX.Class.Repository
{
    public interface IContractRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetBindingContractView(int id);
        dynamic GetContractView(int id);
        dynamic GetDataCalculator(ContractSaveVm vm);
        dynamic Create(ContractVm vm);
        dynamic Update(ContractVm vm);
        dynamic Approve(ContractAppVm vm);
        dynamic Delete(int id);
        dynamic CreateDetail(ContractDetailVm vm);
        dynamic UpdateDetail(ContractDetailVm vm);
        dynamic DeleteDetail(int id);
        dynamic DeleteDetailByMaster(int id);
        dynamic CreateContractTemp(ContractTempVm vm);
        dynamic UpdateContractTemp(ContractTempVm vm);
        dynamic DeleteContractTempByMaster(int id);
        dynamic UpdateContractView(ContractViewVm vm);
    }
    public class ContractRepository : IContractRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ContractRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
            _connectionString = _connectionStrings.ConnectionString;
        }

        public DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"PageIndex", vm.PageIndex},
                {"PageSize", vm.PageSize},
                {"SortItem", vm.SortItem},
                {"SortDirection", vm.SortDirection},
                {"SearchParams", string.IsNullOrEmpty(vm.SearchParams) ? null : vm.SearchParams.Trim()},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sp_frmContractSearch", "SearchPaging");

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
            var resultData = new ContractSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", id}
            };
            var resultM = new StoredProcedureFactory<ContractVm>(_connectionString)
                .FindOneBy(masterParams, "sp_frmContractSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.Contract = resultM.Data;
                //Get temp
                var resultT = new StoredProcedureFactory<ContractTempVm>(_connectionString)
                    .FindAllBy(masterParams, "sp_frmContractSearch", "GetTempByMasterKey");
                if (resultT.Success)
                    resultData.CTemps = new List<ContractTempVm>(resultT.Data.Items);
                //Get detail
                var resultD = new StoredProcedureFactory<ContractDetailVm>(_connectionString)
                    .FindAllBy(masterParams, "sp_frmContractSearch", "GetDetailByMasterKey");
                if (resultD.Success)
                    resultData.CDetails = new List<ContractDetailVm>(resultD.Data.Items);
                return new ApiSuccessResult<ContractSaveVm>(resultData);
            }
            return new ApiErrorResult<ContractSaveVm>();
        }

        public dynamic GetBindingContractView(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", id}
            };
            var resultM = new StoredProcedureFactory<BindingContractViewVm>(_connectionString)
                .FindOneBy(masterParams, "sp_frmContractSearch", "GetBindingContractView");
            return resultM;
        }

        public dynamic GetContractView(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", id}
            };
            var resultM = new StoredProcedureFactory<ContractViewVm>(_connectionString)
                .FindOneBy(masterParams, "sp_frmContractSearch", "GetContractView");
            return resultM;
        }

        public dynamic GetDataCalculator(ContractSaveVm vm)
        {
            var jsonDataDetail = JsonConvert.SerializeObject(vm.CTemps);
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", vm.Contract.ContractDeclareKey},
                {"PartnerKey", vm.Contract.PartnerKey},
                {"ContractCode", vm.Contract.ContractCode},
                {"ContractName", vm.Contract.ContractName},
                {"BuildingKey", vm.Contract.BuildingKey},
                {"BuildingSectionKey", vm.Contract.BuildingSectionKey},
                {"CategoryKey", vm.Contract.CategoryKey},
                {"DepartmentKey", vm.Contract.DepartmentKey},
                {"ContractTermByMonth", vm.Contract.ContractTermByMonth},
                {
                    "ValidDate", string.IsNullOrEmpty(vm.Contract.ValidDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.Contract.ValidDate)
                },
                {"Description", vm.Contract.Description},
                {"Status", vm.Contract.Status},
                {"JsonDataDetail", jsonDataDetail}
            };
            var result = new StoredProcedureFactory<ContractDetailVm>(_connectionString).FindAllBy(masterParams,
                    "sp_frmContractSearch", "GetDataCalculator");
            return result;
        }

        public dynamic Create(ContractVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", vm.ContractDeclareKey},
                {"PartnerKey", vm.PartnerKey},
                {"ContractCode", vm.ContractCode},
                {"ContractName", vm.ContractName},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"CategoryKey", vm.CategoryKey},
                {"DepartmentKey", vm.DepartmentKey},
                {"ContractTermByMonth", vm.ContractTermByMonth},
                {
                    "ValidDate", string.IsNullOrEmpty(vm.ValidDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.ValidDate)
                },
                {"Description", vm.Description},
                {"Status", vm.Status},
                {"OldContractKey", vm.OldContractKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "Create");
            return result;
        }

        public dynamic Update(ContractVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", vm.ContractKey},
                {"ContractDeclareKey", vm.ContractDeclareKey},
                {"PartnerKey", vm.PartnerKey},
                {"ContractCode", vm.ContractCode},
                {"ContractName", vm.ContractName},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"CategoryKey", vm.CategoryKey},
                {"DepartmentKey", vm.DepartmentKey},
                {"ContractTermByMonth", vm.ContractTermByMonth},
                {
                    "ValidDate", string.IsNullOrEmpty(vm.ValidDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.ValidDate)
                },
                {"Description", vm.Description},
                {"Status", vm.Status}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "Update");
            return result;
        }

        public dynamic Approve(ContractAppVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", vm.ContractKey},
                {"Status", vm.Status}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "Approve");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContract", "Delete");
            return result;
        }

        public dynamic CreateDetail(ContractDetailVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", vm.ContractKey},
                {"ContractDeclareDetailKey", vm.ContractDeclareDetailKey},
                {
                    "AnticipatePaymentDate", string.IsNullOrEmpty(vm.AnticipatePaymentDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.AnticipatePaymentDate)
                },
                {"AnticipatePaymentMonth", vm.AnticipatePaymentMonth},
                {"DebtReasonKey", vm.DebtReasonKey},
                {"Amount", vm.Amount},
                {"Description", vm.Description},
                {"Status", vm.Status},
                {"Active", vm.Active},
                {"DepositMonth", vm.DepositMonth},
                {"PenaltyFee", vm.PenaltyFee},
                {"PenaltyFeeAfterDay", vm.PenaltyFeeAfterDay},
                {"IncreaseRentAmount", vm.IncreaseRentAmount},
                {"IncreaseRentPerior", vm.IncreaseRentPerior},
                {"PaymentDate", vm.PaymentDate},
                {"PaymentPeriod", vm.PaymentPeriod}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "CreateDetail");
            return result;
        }

        public dynamic UpdateDetail(ContractDetailVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDetailKey", vm.ContractDetailKey},
                {"ContractKey", vm.ContractKey},
                {"ContractDeclareDetailKey", vm.ContractDeclareDetailKey},
                {
                    "AnticipatePaymentDate", string.IsNullOrEmpty(vm.AnticipatePaymentDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.AnticipatePaymentDate)
                },
                {"AnticipatePaymentMonth", vm.AnticipatePaymentMonth},
                {"DebtReasonKey", vm.DebtReasonKey},
                {"Amount", vm.Amount},
                {"Description", vm.Description},
                {"Status", vm.Status},
                {"Active", vm.Active},
                {"DepositMonth", vm.DepositMonth},
                {"PenaltyFee", vm.PenaltyFee},
                {"PenaltyFeeAfterDay", vm.PenaltyFeeAfterDay},
                {"IncreaseRentAmount", vm.IncreaseRentAmount},
                {"IncreaseRentPerior", vm.IncreaseRentPerior},
                {"PaymentDate", vm.PaymentDate},
                {"PaymentPeriod", vm.PaymentPeriod}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "UpdateDetail");
            return result;
        }

        public dynamic DeleteDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDetailKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContract", "DeleteDetail");
            return result;
        }

        public dynamic DeleteDetailByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContract", "DeleteDetailByMaster");
            return result;
        }

        //ContractView
        public dynamic UpdateContractView(ContractViewVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", vm.ContractKey},
                {"ContractView", vm.ContractView}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "UpdateContractView");
            return result;
        }

        public dynamic CreateContractTemp(ContractTempVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", vm.ContractKey},
                {"ContractDeclareDetailKey", vm.ContractDeclareDetailKey},
                {"DebtReasonKey", vm.DebtReasonKey},
                {"Amount", vm.Amount},
                {"Description", vm.Description},
                {"Status", vm.Status},
                {"Active", vm.Active},
                {"DepositMonth", vm.DepositMonth},
                {"PenaltyFee", vm.PenaltyFee},
                {"PenaltyFeeAfterDay", vm.PenaltyFeeAfterDay},
                {"IncreaseRentAmount", vm.IncreaseRentAmount},
                {"IncreaseRentPerior", vm.IncreaseRentPerior},
                {"PaymentDate", vm.PaymentDate},
                {"PaymentPeriod", vm.PaymentPeriod}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "CreateContractTemp");
            return result;
        }

        public dynamic UpdateContractTemp(ContractTempVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractTempKey", vm.ContractTempKey},
                {"ContractKey", vm.ContractKey},
                {"ContractDeclareDetailKey", vm.ContractDeclareDetailKey},
                {"DebtReasonKey", vm.DebtReasonKey},
                {"Amount", vm.Amount},
                {"Description", vm.Description},
                {"Status", vm.Status},
                {"Active", vm.Active},
                {"DepositMonth", vm.DepositMonth},
                {"PenaltyFee", vm.PenaltyFee},
                {"PenaltyFeeAfterDay", vm.PenaltyFeeAfterDay},
                {"IncreaseRentAmount", vm.IncreaseRentAmount},
                {"IncreaseRentPerior", vm.IncreaseRentPerior},
                {"PaymentDate", vm.PaymentDate},
                {"PaymentPeriod", vm.PaymentPeriod}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContract", "UpdateContractTemp");
            return result;
        }

        public dynamic DeleteContractTempByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContract", "DeleteContractTempByMaster");
            return result;
        }

    }
}
