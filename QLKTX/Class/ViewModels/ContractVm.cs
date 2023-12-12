namespace QLKTX.Class.ViewModels
{
    public class ContractVm
    {
        #region Public Properties
        public int? ContractKey { get; set; }

        public int? ContractDeclareKey { get; set; }

        public int? PartnerKey { get; set; }

        public string ContractCode { get; set; }

        public string ContractName { get; set; }

        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
        public int? CategoryKey { get; set; }

        public string DepartmentKey { get; set; }

        public int ContractTermByMonth { get; set; }

        public string ValidDate { get; set; }

        public string Description { get; set; }

        public bool? Status { get; set; }

        public int? ContractTermByMonthD { get; set; }

        public string ValidDateD { get; set; }
        public string DateExpires { get; set; }
        public int? OldContractKey { get; set; }
        #endregion
    }
}
