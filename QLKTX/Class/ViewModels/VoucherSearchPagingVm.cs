using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class VoucherSearchPagingVm : DataTablePagingSearchVm
    {
        public string TransID { get; set; }
        public string SearchParams { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
        public int? PartnerKey { get; set; }
    }
}
