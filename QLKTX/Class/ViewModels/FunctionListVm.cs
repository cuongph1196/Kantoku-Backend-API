namespace QLKTX.Class.ViewModels
{
    public class FunctionListVm
    {
        #region Public Properties
        public int? ID { get; set; }

        public string FunctionName { get; set; }

        public int ModuleID { get; set; }

        public string Url { get; set; }

        public int Rank { get; set; }

        public bool Display { get; set; }

        public int? ParentID { get; set; }
        public string Icons { get; set; }
        public string TransID { get; set; }
        public bool IsPopup { get; set; }
        #endregion
    }
}
