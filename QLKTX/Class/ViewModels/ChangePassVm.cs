using System.ComponentModel.DataAnnotations;

namespace QLKTX.Class.ViewModels
{
    public class ChangePassVm
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string PasswordOld { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password không khớp.")]
        public string PasswordConfirm { get; set; }

    }
}
