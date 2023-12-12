namespace QLKTX.Class.ViewModels
{
    public class PartnerGroupVm
    {
        #region Public Properties
        public int? PartnerGroupKey { get; set; }

        public string PartnerGroupCode { get; set; }

        public string PartnerGroupName { get; set; }

        public string Description { get; set; }

        public int? PartnerGroupParentKey { get; set; }

        public bool Active { get; set; }
        #endregion
    }
}
