using QLKTX.Class.Entities;

namespace QLKTX.Models
{
    public class ContractVViewModel
    {
        public int ModuleID { get; set; }
        public int FunctionID { get; set; }
        public string AccountLogin { get; set; }
        public int ContractKey { get; set; }
        public UserPermiss FUPermiss { get; set; }

        public ContractVViewModel()
        {
            FUPermiss = new UserPermiss();
        }
    }
}
