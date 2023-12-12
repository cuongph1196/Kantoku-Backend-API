namespace QLKTX.Class.ViewModels
{
    public class DepartmentVm
    {
        #region Public Properties
        public int? DepartmentKey { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public int? BuildingKey { get; set; }

        public int? BuildingSectionKey { get; set; }

        public decimal Price { get; set; }
        #endregion
    }
}
