using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace QLKTX.Class.ViewModels
{
    public class UserAccountVm
    {
        public string? UserID { get; set; }
        public string UserName { get; set; }
        public int? UserGroup { get; set; }
        public int? CompanyStructureKey { get; set; }
        public string CompanyStructureName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
        public bool Active { get; set; }
    }
}
