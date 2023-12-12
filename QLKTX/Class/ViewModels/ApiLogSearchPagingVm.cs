using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class ApiLogSearchPagingVm : DataTablePagingSearchVm
    {
        public string TimeUtcFrom { get; set; }
        public string TimeUtcTo { get; set; }
        public string RequestedMethod { get; set; }
        public string UserID { get; set; }
        public string SearchParams { get; set; }
    }
}
