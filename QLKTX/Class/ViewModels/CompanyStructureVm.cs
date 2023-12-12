namespace QLKTX.Class.ViewModels
{
    public class CompanyStructureVm
    {
        public int? CompanyStructureKey { get; set; }
        public string CompanyStructureCode { get; set; }
        public string CompanyStructureName { get; set; }
        public int? CompanyStructureParent { get; set; }
        public string Description { get; set; }
        public int LevelCompanyStructureKey { get; set; }
        public bool Active { get; set; }
    }
}
