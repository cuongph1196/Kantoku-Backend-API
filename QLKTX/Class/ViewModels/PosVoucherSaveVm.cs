using QLKTX.Class.Entities;

namespace QLKTX.Class.ViewModels
{
    public class PosVoucherSaveVm
    {
        public PosVoucher Voucher { get; set; }
        public List<PosVoucherDetail> VoucherDetails { get; set; }
    }
}
