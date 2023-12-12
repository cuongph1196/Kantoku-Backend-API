using QLKTX.Class.Entities;

namespace QLKTX.Class.ViewModels
{
    public class RefreshTokenResponseVm
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AccessTokenExpires { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
        public string Notes { get; set; }
        public RefreshTokenResponseVm(UserLoginResultVm user, string jwtToken, string refreshToken, string notes)
        {
            AccessToken = jwtToken;
            AccessTokenExpires = user.AccessTokenExpires;
            RefreshTokenExpires = user.RefreshTokenExpires;
            RefreshToken = refreshToken;
            Notes = notes;
        }
    }
}
