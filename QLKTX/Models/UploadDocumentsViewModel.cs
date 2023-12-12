using QLKTX.Class.ViewModels;

namespace QLKTX.Models
{
    public class UploadDocumentsViewModel
    {
        #region Public Properties
        public List<FileUploadVm> Documents { get; set; }
        public bool isDel { get; set; }
        #endregion
    }
}
