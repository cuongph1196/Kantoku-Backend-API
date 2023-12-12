namespace QLKTX.Class.ViewModels
{
    public class PartnerVm
    {
        #region Public Properties
        public int? PartnerKey { get; set; }

        public string PartnerCode { get; set; }

        public string PartnerName { get; set; }

        public string PartnerNonUnicodeSearch { get; set; }

        public string IdentityID { get; set; }

        public string IdentityDateIssue { get; set; }

        public string IdentityPlaceIssue { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string PartnerAddress { get; set; }

        public string PartnerPhone { get; set; }

        public string PartnerTaxNumber { get; set; }

        public string PartnerTaxName { get; set; }

        public string PartnerTaxAddress { get; set; }

        public int? PartnerGroupkey { get; set; }

        public bool? IsEmployee { get; set; }

        public bool? IsCustomer { get; set; }

        public bool? IsVendor { get; set; }

        public string NFCCode { get; set; }

        public int? BuildingKey { get; set; }
        #endregion
    }
}
