using QLKTX.Class.Enums;
using QLKTX.Class.ViewModels;
using System.Reflection;

namespace QLKTX.Class.Entities
{
    public class LoggedInUser
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int? UserGroup { get; set; }
        public Role Role { get; set; } = Role.User; //default 
        public bool FAdm { get; set; } = false; //default 
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
        public DateTime? AccessTokenExpires { get; set; }

        public LoggedInUser()
        {
        }

        public LoggedInUser (UserLoginResultVm user, string jwtToken, string refreshToken)
        {
            UserID = user.UserID;
            UserName = user.UserName;
            UserGroup = user.UserGroup;
            Role = user.Role;
            FAdm = user.FAdm;
            AccessToken = jwtToken;
            AccessTokenExpires = user.AccessTokenExpires;
            RefreshTokenExpires = user.RefreshTokenExpires;
            RefreshToken = refreshToken;
        }
    }
}
