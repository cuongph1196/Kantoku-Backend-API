namespace QLKTX.Class.ViewModels
{
    public class BuildingVm
    {
        #region Public Properties
        public int? BuildingKey { get; set; }

        public string BuildingCode { get; set; }

        public string BuildingName { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string TaxNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public int CompanyStructureKey { get; set; }
        #endregion
    }
}
