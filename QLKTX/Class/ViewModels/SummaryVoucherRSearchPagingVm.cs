using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class SummaryVoucherRSearchPagingVm : DataTablePagingSearchVm
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int BuildingKey { get; set; }
        public int FunctionID { get; set; }
    }
}
