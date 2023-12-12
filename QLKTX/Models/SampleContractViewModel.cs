using QLKTX.Class.Entities;

namespace QLKTX.Models
{
    public class SampleContractViewModel
    {
        public int ModuleID { get; set; }
        public int FunctionID { get; set; }
        public string AccountLogin { get; set; }
        public int ContractDeclareKey { get; set; }
        public UserPermiss FUPermiss { get; set; }

        public SampleContractViewModel()
        {
            FUPermiss = new UserPermiss();
        }
    }
}
