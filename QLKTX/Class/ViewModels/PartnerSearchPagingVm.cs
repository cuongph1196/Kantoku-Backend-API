using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class PartnerSearchPagingVm : DataTablePagingSearchVm
    {
        public string SearchParams { get; set; }
        public int? PartnerGroupkey { get; set; }
    }
}
