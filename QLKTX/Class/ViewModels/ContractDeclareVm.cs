namespace QLKTX.Class.ViewModels
{
    public class ContractDeclareVm
    {
        #region Public Properties
        public int? ContractDeclareKey { get; set; }

        public string ContractDeclareCode { get; set; }

        public string ContractDeclareName { get; set; }

        public int? BuildingKey { get; set; }
        public int? BuildingSectionKey { get; set; }
        public int? CategoryKey { get; set; }

        public int ContractTermByMonth { get; set; }

        public string ValidDate { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }
        #endregion
    }
}
