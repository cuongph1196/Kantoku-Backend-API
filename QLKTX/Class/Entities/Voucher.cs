namespace QLKTX.Class.Entities
{
    public class Voucher
    {
        #region Public Properties
        public int? Rowkey { get; set; }

        public string TransNo { get; set; }

        public string TransDate { get; set; }

        public string TransID { get; set; }

        public int? PartnerKey { get; set; }
        public string PartnerName { get; set; }

        public string PartnerTaxNumber { get; set; }

        public string PartnerTaxName { get; set; }

        public string PartnerTaxAddress { get; set; }

        public string TaxTemplateNo { get; set; }

        public string TaxSerialNo { get; set; }

        public string TaxNo { get; set; }

        public string TaxDate { get; set; }

        public int? EmployeeKey { get; set; }

        public int? POSKey { get; set; }

        public int? CashierKey { get; set; }

        public int? BuildingKey { get; set; }

        //public string DepartmentKey { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; } = false;

        public bool IsDeleted { get; set; }

        public bool IsCreatedDebt { get; set; }

        public int? OldRowkey { get; set; }

        public string OldTransNo { get; set; }

        public string OldTransID { get; set; }
        #endregion
    }
}
