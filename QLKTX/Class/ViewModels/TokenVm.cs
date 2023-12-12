using System.ComponentModel.DataAnnotations;

namespace QLKTX.Class.ViewModels
{
    public class TokenVm
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
