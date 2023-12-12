namespace QLKTX.Class.ViewModels
{
    public class ReasonVm
    {
        #region Public Properties
        public int ReasonKey { get; set; }

        public string ReasonCode { get; set; }

        public string ReasonName { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string ReasonParentID { get; set; }

        public string RecordID { get; set; }
        #endregion
    }
}
