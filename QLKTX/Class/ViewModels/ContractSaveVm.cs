namespace QLKTX.Class.ViewModels
{
    public class ContractSaveVm
    {
        public ContractVm Contract { get; set; }
        public List<ContractTempVm> CTemps { get; set; }
        public List<ContractDetailVm> CDetails { get; set; }
    }
}
