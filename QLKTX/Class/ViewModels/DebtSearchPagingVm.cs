using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class DebtSearchPagingVm : DataTablePagingSearchVm
    {
        public string TransID { get; set; }
        public string SearchParams { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
