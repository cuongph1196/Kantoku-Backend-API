namespace QLKTX.Class.ViewModels
{
    public class FileUploadVm
    {
        #region Public Properties
        public int? ID { get; set; }

        public string TransID { get; set; }

        public int MasterKey { get; set; }

        public int? DetailKey { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileUrl { get; set; }
        #endregion
    }
}
