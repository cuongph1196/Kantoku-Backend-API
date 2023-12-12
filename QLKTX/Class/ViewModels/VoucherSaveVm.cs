using QLKTX.Class.Entities;

namespace QLKTX.Class.ViewModels
{
    public class VoucherSaveVm
    {
        public Voucher Voucher { get; set; }
        public List<VoucherDetail> VoucherDetails { get; set; }
    }
}
