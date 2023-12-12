namespace QLKTX.Class.ViewModels
{
    public class PrintVoucherVm
    {
        public int Rowkey { get; set; }
        public string TransNo { get; set; }
        public string TransDate { get; set; }
        public string TransID { get; set; }
        public string TransName { get; set; }
        public string PartnerName { get; set; }
        public string PartnerTaxNumber { get; set; }
        public string PartnerTaxName { get; set; }
        public string PartnerTaxAddress { get; set; }
        public string BuildingTaxName { get; set; }
        public string BuildingTaxNumber { get; set; }
        public string BuildingTaxAddress { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string AmountText { get; set; }
    }
}
