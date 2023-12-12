using System.ComponentModel.DataAnnotations;

namespace QLKTX.Class.ViewModels
{
    public class UserLoginVm
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }
    }
}
