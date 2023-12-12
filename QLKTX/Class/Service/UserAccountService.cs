using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using QLKTX.Class.Authorization;
using QLKTX.Class.Exceptions;
using QLKTX.Class.Helper;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Controllers.Mobile;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace QLKTX.Class.Service
{
    public interface IUserAccountService
    {
        RefreshTokenResponseVm RefreshToken(TokenVm tokenModel); //lấy access token mới
        string DecodeStr(string str); //decode string
    }
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly IPosDeviceRepository _posDeviceRepository;
        private readonly IConfiguration _configuration;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private IMemoryCache _cache;

        public UserAccountService(
            IConfiguration configuration,
            IMemoryCache cache,
            IUserAccountRepository userAccountRepository,
            IUserPermissionRepository userPermissionRepository,
            IPosDeviceRepository posDeviceRepository,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _configuration = configuration;
            _cache = cache;
            _userAccountRepository = userAccountRepository;
            _userPermissionRepository = userPermissionRepository;
            _posDeviceRepository = posDeviceRepository;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        public RefreshTokenResponseVm RefreshToken(TokenVm tokenModel)
        {
            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            string checkAccessToken = _jwtUtils.ValidateJwtToken(accessToken);
            if (checkAccessToken == "")
            {
                throw new AppException("Access token sai format");
            }
            //hunglt bỏ qua không check access token còn hạn. Cứ gọi refesh token là sẽ trả ra token mới cho user.
            //Vì có thể user sẽ gọi trước khi hết hạn.
            //if (checkAccessToken == "OK")
            //{
            //    throw new AppException("Access token vẫn còn hạn");
            //}

            //khi access token hết hạn thì mới cho refesh. Cập nhật còn hạn vẫ cấp mới refesh token
            var result = _userAccountRepository.GetUserByRefreshToken(refreshToken);
            if (!result.Success)
            {
                //khi Refresh token ko đúng, hết hạn, update không được thì trả ra 403 cho nó đăng nhập lại
                //Refresh token did not match users or user is already logged out
                throw new AppException("REFRESHTOKENEXPIRES");
            }
            var userMember = new UserLoginResultVm();
            userMember = result.Data;
            if (userMember.RefreshToken != refreshToken || userMember.RefreshTokenExpires <= new DateTimeOffset(DateTime.Now).DateTime)
            {
                //khi Refresh token ko đúng, hết hạn, update không được thì trả ra 403 cho nó đăng nhập lại
                throw new AppException("REFRESHTOKENEXPIRES");
            }
            //new token
            JwtSecurityToken jwtSecurityToken = _jwtUtils.GenerateJwtToken(userMember);
            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var newRefreshToken = _jwtUtils.CreateRefreshToken();

            userMember.RefreshToken = newRefreshToken.Token;
            userMember.RefreshTokenExpires = newRefreshToken.Expires;
            var updateResult = _userAccountRepository.UpdateRefreshToken(userMember);

            var validAccessTokenExpires = jwtSecurityToken.ValidTo.ToString();
            DateTime convertedDate = DateTime.SpecifyKind(DateTime.Parse(validAccessTokenExpires), DateTimeKind.Utc);
            DateTime dtAccessTokenExpires = convertedDate.ToLocalTime();
            userMember.AccessTokenExpires = dtAccessTokenExpires;
            if (updateResult.Success)
            {
                return new RefreshTokenResponseVm(userMember, newAccessToken, newRefreshToken.Token, "Cấp mới token");
            }
            //khi Refresh token ko đúng, hết hạn, update không được thì trả ra 401 cho nó đăng nhập lại
            throw new AppException("REFRESHTOKENEXPIRES"); //lỗi không cấp được refresh token thì nên login lại
        }

        public string DecodeStr(string str)
        {
            string decodeStr = Encoding.UTF8.GetString(Convert.FromBase64String(str));
            return decodeStr;
        }
    }
}
