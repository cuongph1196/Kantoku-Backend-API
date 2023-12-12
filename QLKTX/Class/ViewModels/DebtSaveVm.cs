using QLKTX.Class.Entities;

namespace QLKTX.Class.ViewModels
{
    public class DebtSaveVm
    {
        public Debt Debt { get; set; }
        public List<DebtDetail> DebtDetails { get; set; }
    }
}
