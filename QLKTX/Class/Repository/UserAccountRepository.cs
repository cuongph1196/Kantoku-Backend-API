using BCryptNet = BCrypt.Net.BCrypt;
using Dapper;
using Microsoft.Extensions.Options;
using QLKTX.Class.ViewModels;
using System.Data.SqlClient;
using System.Data;
using QLKTX.Class.ViewModels.Base;
using QLKTX.Class.Helper;
using QLKTX.Class.Dtos;

namespace QLKTX.Class.Repository
{
    public interface IUserAccountRepository
    {
        DataTablePagingResultVm<dynamic> SearchPaging(SearchPagingVm vm);
        dynamic GetById(string id);
        dynamic Create(UserAccountVm vm);
        dynamic Update(UserAccountVm vm);
        dynamic UpdateName(UserAccountVm vm);
        dynamic Delete(string id);
        dynamic Authenticate(UserLoginVm vm);

        dynamic UpdatePass(ChangePassVm vm);
        dynamic AuthenticateM(UserLoginVm vm);
        dynamic GetUserByRefreshToken(string token);
        dynamic UpdateRefreshToken(UserLoginResultVm userMember);

    }
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly string _connectionString;
        private readonly ConnectionStrings _connectionStrings;

        public UserAccountRepository(IConfiguration configuration, IOptions<ConnectionStrings> connectionStrings)
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
                "spfrm_UserAccountSearch", "SearchPaging");

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

        public dynamic GetById(string id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", id}
            };
            var result = new StoredProcedureFactory<UserAccountVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_UserAccountSearch", "GetByID");
            return result;
        }

        private dynamic LoginByID(string id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", id}
            };
            var result = new StoredProcedureFactory<UserLoginResultVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_UserAccountSearch", "LoginByID");
            return result;
        }

        public dynamic Create(UserAccountVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserName", vm.UserName},
                {"UserGroup", vm.UserGroup},
                {"CompanyStructureKey", vm.CompanyStructureKey},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_UserAccount", "Create");
            return result;
        }

        public dynamic Update(UserAccountVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", vm.UserID},
                {"UserName", vm.UserName},
                {"UserGroup", vm.UserGroup},
                {"CompanyStructureKey", vm.CompanyStructureKey},
                {"Active", vm.Active}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_UserAccount", "Update");
            return result;
        }

        public dynamic UpdateName(UserAccountVm vm)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", vm.UserID},
                {"UserName", vm.UserName}
            };
            var result = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                    "spfrm_UserAccount", "UpdateName");
            return result;
        }

        public dynamic UpdatePass(ChangePassVm vm)
        {
            var user = new UserLoginResultVm();
            var res = this.LoginByID(vm.UserID);
            user = res.Success ? res.Data : null;
            if (user == null || !BCryptNet.Verify(vm.PasswordOld, user.Password))
            {
                return new ApiErrorResult<string>("Mật khẩu không chính xác");
            }
            else
            {
                var masterParams = new Dictionary<string, object>
                {
                    {"UserID", vm.UserID},
                    {"Password", BCryptNet.HashPassword(vm.Password)}
                };
                var resultP = new StoredProcedureFactory<string>(_connectionString).msgExecute(masterParams,
                        "spfrm_UserAccount", "ChangePassword");
                return resultP;
            }
        }

        public dynamic Delete(string id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", id}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams,
                "spfrm_UserAccount", "Delete");
            return result;
        }


        public dynamic Authenticate(UserLoginVm vm)
        {
            var result = new ApiResult<UserLoginResultVm>();
            var user = new UserLoginResultVm();
            var res = this.LoginByID(vm.UserID);
            //var a = BCryptNet.HashPassword(vm.Password);
            // validate
            user = res.Success ? res.Data : null;
            if (user == null || !BCryptNet.Verify(vm.Password, user.Password))
            {
                result = new ApiErrorResult<UserLoginResultVm>("Tài khoản đăng nhập hoặc mật khẩu không chính xác");
            }
            else
            {
                result = new ApiSuccessResult<UserLoginResultVm>(user);
            }
            return result;
        }

        public dynamic AuthenticateM(UserLoginVm vm)
        {
            var result = new ApiResult<UserLoginResultVm>();
            var user = new UserLoginResultVm();
            var res = this.MLoginByID(vm.UserID);
            //var a = BCryptNet.HashPassword(vm.Password);
            // validate
            user = res.Success ? res.Data : null;
            if (user == null || !BCryptNet.Verify(vm.Password, user.Password))
            {
                result = new ApiErrorResult<UserLoginResultVm>("Tài khoản đăng nhập hoặc mật khẩu không chính xác");
            }
            else
            {
                result = new ApiSuccessResult<UserLoginResultVm>(user);
            }
            return result;
        }

        private dynamic MLoginByID(string id)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", id}
            };
            var result = new StoredProcedureFactory<UserLoginResultVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_UserAccountSearch", "MobileLoginByID");
            return result;
        }

        public dynamic GetUserByRefreshToken(string token)
        {
            var masterParams = new Dictionary<string, object>();
            masterParams.Add("Token", string.IsNullOrEmpty(token) ? null : token);
            var result = new StoredProcedureFactory<UserLoginResultVm>(_connectionString).FindOneBy(masterParams,
                "spfrm_UserAccountSearch", "GetUserByRefreshToken");
            return result;
        }

        public dynamic UpdateRefreshToken(UserLoginResultVm userMember)
        {
            var masterParams = new Dictionary<string, object>
            {
                {"UserID", userMember.UserID},
                {"RefreshToken", userMember.RefreshToken},
                {"RefreshTokenExpires", userMember.RefreshTokenExpires}
            };
            var result = new StoredProcedureFactory<int>(_connectionString).intExecute(masterParams, "spfrm_UserAccount", "UpdateRefreshToken");
            return result;
        }
    }
}
