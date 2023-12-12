namespace QLKTX.Class.ViewModels
{
    public class BindingContractViewVm
    {
        public int ContractKey { get; set; }
        public int ContractDeclareKey { get; set; }
        public int PartnerKey { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public int BuildingKey { get; set; }
        public string BuildingName { get; set; }
        public string BuildingAddress { get; set; }
        public string DepartmentKey { get; set; }
        public int ContractTermByMonth { get; set; }
        public string ValidDate { get; set; }
        public decimal DepositMonth { get; set; }
        public decimal PenaltyFee { get; set; }
        public decimal PenaltyFeeAfterDay { get; set; }
        public decimal IncreaseRentAmount { get; set; }
        public decimal IncreaseRentPerior { get; set; }
        public int PaymentPeriod { get; set; }
        public int PaymentDate { get; set; }
        public string Description { get; set; }
        public string SampleContract { get; set; }
        public string PartnerName { get; set; }
        public string IdentityID { get; set; }
        public string IdentityDateIssue { get; set; }
        public string IdentityPlaceIssue { get; set; }
        public string PartnerAddress { get; set; }
        public string PartnerTaxNumber { get; set; }
        public string PartnerPhone { get; set; }
        public bool Status { get; set; }
    }
}
