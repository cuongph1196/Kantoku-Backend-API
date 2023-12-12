using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class DepartmentSearchPagingVm : DataTablePagingSearchVm
    {
        public string SearchParams { get; set; }
        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
    }
}
