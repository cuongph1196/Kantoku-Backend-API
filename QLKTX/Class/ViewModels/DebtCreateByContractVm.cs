namespace QLKTX.Class.ViewModels
{
    public class DebtCreateByContractVm
    {
        public int BuildingKey { get; set; }
        public int DepartmentKey { get; set; }
        public int PartnerKey { get; set; }
        public string PartnerTaxNumber { get; set; }
        public string PartnerTaxName { get; set; }
        public string PartnerTaxAddress { get; set; }
        public int ReasonKey { get; set; }
        public decimal Amount { get; set; }
        public string TransDate { get; set; }
        public int ContractDetailKey { get; set; }
        public int ContractKey { get; set; }
    }
}
