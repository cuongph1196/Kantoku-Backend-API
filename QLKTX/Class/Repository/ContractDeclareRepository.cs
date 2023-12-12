using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;

namespace QLKTX.Class.Repository
{
    public interface IContractDeclareRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetDetailById(int id);
        dynamic Create(ContractDeclareVm vm);
        dynamic Update(ContractDeclareVm vm);
        dynamic Delete(int id);
        dynamic CreateDetail(ContractDeclareDetailVm vm);
        dynamic UpdateDetail(ContractDeclareDetailVm vm);
        dynamic DeleteDetail(int id);
        dynamic DeleteDetailByMaster(int id);

        dynamic GetSampleContract(int id);
        dynamic UpdateSampleContract(SampleContractVm vm);
    }
    public class ContractDeclareRepository : IContractDeclareRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public ContractDeclareRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "sp_frmContractDeclareSearch", "SearchPaging");

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
            var resultData = new ContractDeclareSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", id}
            };
            var resultM = new StoredProcedureFactory<ContractDeclareVm>(_connectionString)
                .FindOneBy(masterParams, "sp_frmContractDeclareSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.ContractDeclare = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<ContractDeclareDetailVm>(_connectionString)
                    .FindAllBy(masterParams, "sp_frmContractDeclareSearch", "GetDetailByMasterKey");
                if (resultD.Success)
                    resultData.CDDetails = new List<ContractDeclareDetailVm>(resultD.Data.Items);
                return new ApiSuccessResult<ContractDeclareSaveVm>(resultData);
            }
            return new ApiErrorResult<ContractDeclareSaveVm>();
        }

        public dynamic GetDetailById(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", id}
            };
            //Get detail
            var resultD = new StoredProcedureFactory<ContractDeclareDetailVm>(_connectionString)
                .FindAllBy(masterParams, "sp_frmContractDeclareSearch", "GetDetailByMasterKey");
            return resultD;
        }

        public dynamic Create(ContractDeclareVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareCode", vm.ContractDeclareCode},
                {"ContractDeclareName", vm.ContractDeclareName},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"CategoryKey", vm.CategoryKey},
                {"ContractTermByMonth", vm.ContractTermByMonth},
                {
                    "ValidDate", string.IsNullOrEmpty(vm.ValidDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.ValidDate)
                },
                {"Description", vm.Description},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContractDeclare", "Create");
            return result;
        }

        public dynamic Update(ContractDeclareVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", vm.ContractDeclareKey},
                {"ContractDeclareCode", vm.ContractDeclareCode},
                {"ContractDeclareName", vm.ContractDeclareName},
                {"BuildingKey", vm.BuildingKey},
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"CategoryKey", vm.CategoryKey},
                {"ContractTermByMonth", vm.ContractTermByMonth},
                {
                    "ValidDate", string.IsNullOrEmpty(vm.ValidDate)
                        ? null
                        : StringFormatDateHelper.ConvertDate(vm.ValidDate)
                },
                {"Description", vm.Description},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContractDeclare", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContractDeclare", "Delete");
            return result;
        }

        public dynamic CreateDetail(ContractDeclareDetailVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", vm.ContractDeclareKey},
                {"DebtReasonKey", vm.DebtReasonKey},
                {"Amount", vm.Amount},
                {"Description", vm.Description},
                {"Active", vm.Active},
                {"DepositMonth", vm.DepositMonth},
                {"PenaltyFee", vm.PenaltyFee},
                {"PenaltyFeeAfterDay", vm.PenaltyFeeAfterDay},
                {"IncreaseRentAmount", vm.IncreaseRentAmount},
                {"IncreaseRentPerior", vm.IncreaseRentPerior},
                {"PaymentPeriod", vm.PaymentPeriod},
                {"PaymentDate", vm.PaymentDate}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).intExecute(masterParams,
                    "sp_frmContractDeclare", "CreateDetail");
            return result;
        }

        public dynamic UpdateDetail(ContractDeclareDetailVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareDetailKey", vm.ContractDeclareDetailKey},
                {"ContractDeclareKey", vm.ContractDeclareKey},
                {"DebtReasonKey", vm.DebtReasonKey},
                {"Amount", vm.Amount},
                {"Description", vm.Description},
                {"Active", vm.Active},
                {"DepositMonth", vm.DepositMonth},
                {"PenaltyFee", vm.PenaltyFee},
                {"PenaltyFeeAfterDay", vm.PenaltyFeeAfterDay},
                {"IncreaseRentAmount", vm.IncreaseRentAmount},
                {"IncreaseRentPerior", vm.IncreaseRentPerior},
                {"PaymentPeriod", vm.PaymentPeriod},
                {"PaymentDate", vm.PaymentDate}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContractDeclare", "UpdateDetail");
            return result;
        }

        public dynamic DeleteDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareDetailKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContractDeclare", "DeleteDetail");
            return result;
        }

        public dynamic DeleteDetailByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmContractDeclare", "DeleteDetailByMaster");
            return result;
        }

        //SampleContract
        public dynamic GetSampleContract(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", id}
            };
            var result = new StoredProcedureFactory<SampleContractVm>(_connectionString)
                .FindOneBy(masterParams, "sp_frmContractDeclareSearch", "GetSampleContract");
            
            return result;
        }

        public dynamic UpdateSampleContract(SampleContractVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"ContractDeclareKey", vm.ContractDeclareKey},
                {"SampleContract", vm.SampleContract}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmContractDeclare", "UpdateSampleContract");
            return result;
        }
    }
}
