using QLKTX.Class.ViewModels.Base;

namespace QLKTX.Class.ViewModels
{
    public class ContractExpiresSearchPagingVm : DataTablePagingSearchVm
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
    }
}
