namespace QLKTX.Class.ViewModels.Base
{
    public class DataTablePagingSearchVm
    {
        public int Draw { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortItem { get; set; }
        public string SortDirection { get; set; }
        public int FunctionID { get; set; }
    }
}
