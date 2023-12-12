using Microsoft.Extensions.Options;
using QLKTX.Class.Dtos;
using QLKTX.Class.Helper;
using QLKTX.Class.ViewModels;
using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.Repository
{
    public interface IBuildingRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic GetDetail(int id);
        dynamic GetDetailImageByID(int id);
        dynamic Create(BuildingVm vm);
        dynamic Update(BuildingVm vm);
        dynamic Delete(int id);
        dynamic CreateDetail(BuildingSectionVm vm);
        dynamic UpdateDetail(BuildingSectionVm vm);
        dynamic DeleteDetail(int id);
        dynamic DeleteDetailByMaster(int id);
        dynamic UpdateDetailImg(BuildingSectionImgVm vm);
    }
    public class BuildingRepository : IBuildingRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public BuildingRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                {"FunctionID", vm.FunctionID}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindAllBy(masterParams,
                "sp_frmBuildingSearch", "SearchPaging");

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
            var resultData = new BuildingSaveVm();
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingKey", id}
            };
            var resultM = new StoredProcedureFactory<BuildingVm>(_connectionString)
                .FindOneBy(masterParams, "sp_frmBuildingSearch", "GetByID");
            if (resultM.Success)
            {
                resultData.Building = resultM.Data;
                //Get detail
                var resultD = new StoredProcedureFactory<BuildingSectionVm>(_connectionString)
                    .FindAllBy(masterParams, "sp_frmBuildingSearch", "GetDetailByMasterKey");
                if (resultD.Success)
                    resultData.BuildingSections = new List<BuildingSectionVm>(resultD.Data.Items);
                return new ApiSuccessResult<BuildingSaveVm>(resultData);
            }
            return new ApiErrorResult<BuildingSaveVm>();
        }

        public dynamic GetDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingKey", id}
            };
            var result = new StoredProcedureFactory<BuildingSectionVm>(_connectionString).FindAllBy(masterParams,
                "sp_frmBuildingSearch", "GetDetailByMasterKey");
            return result;
        }

        public dynamic GetDetailImageByID(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingSectionKey", id}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindOneBy(masterParams,
                "sp_frmBuildingSearch", "GetImageByID");
            return result;
        }

        public dynamic Create(BuildingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingCode", vm.BuildingCode},
                {"BuildingName", vm.BuildingName},
                {"Description", vm.Description},
                {"Active", vm.Active},
                {"TaxNumber", vm.TaxNumber},
                {"CompanyName", vm.CompanyName},
                {"Address", vm.Address},
                {"CompanyStructureKey", vm.CompanyStructureKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmBuilding", "Create");
            return result;
        }

        public dynamic Update(BuildingVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingKey", vm.BuildingKey},
                {"BuildingCode", vm.BuildingCode},
                {"BuildingName", vm.BuildingName},
                {"Description", vm.Description},
                {"Active", vm.Active},
                {"TaxNumber", vm.TaxNumber},
                {"CompanyName", vm.CompanyName},
                {"Address", vm.Address},
                {"CompanyStructureKey", vm.CompanyStructureKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmBuilding", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmBuilding", "Delete");
            return result;
        }

        public dynamic CreateDetail(BuildingSectionVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingSectionCode", vm.BuildingSectionCode},
                {"BuildingSectionName", vm.BuildingSectionName},
                {"Description", vm.Description},
                {"Active", vm.Active},
                //{"BuildingSectionImg", vm.BuildingSectionImg},
                {"BuildingKey", vm.BuildingKey}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "sp_frmBuilding", "CreateDetail");
            return result;
        }

        public dynamic UpdateDetail(BuildingSectionVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"BuildingSectionCode", vm.BuildingSectionCode},
                {"BuildingSectionName", vm.BuildingSectionName},
                {"Description", vm.Description},
                {"Active", vm.Active},
                //{"BuildingSectionImg", vm.BuildingSectionImg},
                {"BuildingKey", vm.BuildingKey}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmBuilding", "UpdateDetail");
            return result;
        }

        public dynamic DeleteDetail(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingSectionKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmBuilding", "DeleteDetail");
            return result;
        }

        public dynamic DeleteDetailByMaster(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "sp_frmBuilding", "DeleteDetailByMaster");
            return result;
        }

        public dynamic UpdateDetailImg(BuildingSectionImgVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"BuildingSectionKey", vm.BuildingSectionKey},
                {"BuildingSectionImg", vm.BuildingSectionImg}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "sp_frmBuilding", "UpdateDetailImg");
            return result;
        }
    }
}
