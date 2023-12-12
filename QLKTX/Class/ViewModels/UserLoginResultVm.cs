
using QLKTX.Class.Enums;
using System.Text.Json.Serialization;

namespace QLKTX.Class.ViewModels
{
    public class UserLoginResultVm
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? UserGroup { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
        public DateTime? AccessTokenExpires { get; set; }
        public Role Role { get; set; } = Role.User; //default 
        public bool FAdm { get; set; } = false; //default 
    }
}
