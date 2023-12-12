namespace QLKTX.Class.ViewModels.Base
{
    public class SearchPagingVm : DataTablePagingSearchVm
    {
        public string SearchParams { get; set; }
        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
    }
}
