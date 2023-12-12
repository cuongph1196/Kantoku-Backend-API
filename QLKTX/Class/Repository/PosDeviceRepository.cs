using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.ViewModels;
using Microsoft.Extensions.Options;
using QLKTX.Class.Helper;
using QLKTX.Class.Dtos;

namespace QLKTX.Class.Repository
{
    public interface IPosDeviceRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(int id);
        dynamic Create(PosDeviceVm vm);
        dynamic Update(PosDeviceVm vm);
        dynamic Delete(int id);
        dynamic CheckDevice(string deviceCode);
    }
    public class PosDeviceRepository: IPosDeviceRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public PosDeviceRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_PosDeviceSearch", "SearchPaging");

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
                {"DeviceKey", id}
            };
            var result = new StoredProcedureFactory<PosDeviceVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_PosDeviceSearch", "GetByID");
            return result;
        }

        public dynamic CheckDevice(string deviceCode)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DeviceCode", deviceCode}
            };
            var result = new StoredProcedureFactory<dynamic>(_connectionString).FindOneBy(masterParams,
                "spfrm_PosDeviceSearch", "CheckDevice");
            if (result.Success)
            {
                var ischeck = result.Data?.Device;
                return ischeck ? 
                    new ApiSuccessResult<bool>("Thiết bị đã được kích hoạt !!!"):
                    new ApiErrorResult<bool>("Thiết bị chưa được kích hoạt !!!");
            }
            else
            {
                return new ApiErrorResult<bool>("Thiết bị chưa được đăng ký !!!");
            }
        }

        public dynamic Create(PosDeviceVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DeviceCode", vm.DeviceCode},
                {"DeviceName", vm.DeviceName},
                {"DeviceDesc", vm.DeviceDesc},
                {"Device_Seri", vm.Device_Seri},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_PosDevice", "Create");
            return result;
        }

        public dynamic Update(PosDeviceVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DeviceKey", vm.DeviceKey},
                {"DeviceCode", vm.DeviceCode},
                {"DeviceName", vm.DeviceName},
                {"DeviceDesc", vm.DeviceDesc},
                {"Device_Seri", vm.Device_Seri},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                    "spfrm_PosDevice", "Update");
            return result;
        }

        public dynamic Delete(int id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"DeviceKey", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_PosDevice", "Delete");
            return result;
        }
    }
}
