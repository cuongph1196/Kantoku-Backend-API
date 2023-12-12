using System.ComponentModel.DataAnnotations;

namespace QLKTX.Class.Entities
{
    public class RefreshToken
    {
        [Key]
        public string UserID { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.Now >= Expires;
        public DateTime Created { get; set; }
    }
}
