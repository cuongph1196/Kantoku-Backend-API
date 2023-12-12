using QLKTX.Class.ViewModels;

namespace QLKTX.Models
{
    public class PrintVoucherViewModel
    {
        public int ModuleID { get; set; }
        public int FunctionID { get; set; }
        public string AccountLogin { get; set; }
        public int VoucherKey { get; set; }
        public PrintVoucherVm PrintV { get; set; }

        public PrintVoucherViewModel()
        {
            PrintV = new PrintVoucherVm();
        }
    }
}
