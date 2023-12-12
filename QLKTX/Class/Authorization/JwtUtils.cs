using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using QLKTX.Class.Entities;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Class.Helper;
using QLKTX.Class.Exceptions;

namespace QLKTX.Class.Authorization
{
    public interface IJwtUtils
    {
        public JwtSecurityToken GenerateJwtToken(UserLoginResultVm user);
        public string ValidateJwtToken(string token);
        public RefreshToken CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;
        private readonly IUserAccountRepository _userMemberRepository;
        public JwtUtils(
            IOptions<AppSettings> appSettings,
            IUserAccountRepository userMemberRepository
        )
        {
            _appSettings = appSettings.Value;
            _userMemberRepository = userMemberRepository;
        }

        public JwtSecurityToken GenerateJwtToken(UserLoginResultVm user)
        {
            //generate token that is valid for 1 ngày
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var Issuer = _appSettings.Issuer;
            var Audience = _appSettings.Audience;
            var TokenExpires = _appSettings.TokenExpires; //token hết hạn trong bao lâu. tính theo phút cho dễ debug
            var userMember = new LoggedInUser();
            userMember.UserID = user.UserID;
            userMember.UserName = user.UserName;
            userMember.UserGroup = user.UserGroup;
            userMember.Role = user.Role;
            userMember.FAdm = user.FAdm;
            IEnumerable<Claim> authClaims = new Claim[] {
                new Claim(ClaimTypes.Name, user.UserID),
                new Claim("UserLogin", JsonConvert.SerializeObject(userMember)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                expires: new DateTimeOffset(DateTime.Now).DateTime.AddMinutes(TokenExpires),
                claims: authClaims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature) //HmacSha256
            );
            return token;
        }
        public string ValidateJwtToken(string token)
        {
            string pattern_tokenExpries = PatternConstant.PATTERN_TOKENEXPRIES;
            //hàm này để check access token khi refresh token. xem là còn hạn hay không mới cho làm mới.
            if (token == null)
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var Issuer = _appSettings.Issuer;
            var Audience = _appSettings.Audience;
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    SaveSigninToken = true,
                    ValidateActor = true,
                    ValidateIssuer = true, //false dev
                    ValidateAudience = true, //false dev
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return "OK"; //còn hạn
            }
            catch (Exception ex)
            {
                //trả ra lỗi xem hết hạn hay là sai format,etc...               
                Match match_token = Regex.Match(ex.Message.ToString(), pattern_tokenExpries);
                if (match_token.Success)
                {
                    return "IDX10223"; //hết hạn access token thì mới cấp mới
                }
                return ""; //sai format or ...IDX10503
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            if (token == null)
                return null; // The parameter 'token' cannot be a 'null' or an empty object. (Parameter 'token')
            try
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var Issuer = _appSettings.Issuer;
                var Audience = _appSettings.Audience;
                var tokenValidationParameters = new TokenValidationParameters
                {
                    SaveSigninToken = true,
                    ValidateActor = true,
                    ValidateIssuer = true, //false dev
                    ValidateAudience = true, //false dev
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                //nếu token hết hạn thì chỗ này sẽ nhảy ra Exception
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                //end token hết hạn

                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                    throw new ForbiddenCustomException("IDX10223");
                return principal;
            }
            catch (Exception)
            {
                // return null if validation fails
                throw new ForbiddenCustomException("IDX10223"); //The token is expired
            }
        }
        public RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = new DateTimeOffset(DateTime.Now).DateTime.AddDays(_appSettings.RefreshTokenCookie),
                    Created = new DateTimeOffset(DateTime.Now).DateTime
                };

            }
        }
    }
}
