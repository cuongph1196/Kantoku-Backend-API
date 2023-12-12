using QLKTX.Class.Entities;

namespace QLKTX.Models
{
    public class PageInfoViewModel
    {
        public int ModuleID { get; set; }
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string AccountLogin { get; set; }
        public string TransID { get; set; }
        public string ClosingDate { get; set; }
        public UserPermiss FUPermiss { get; set; }

        public PageInfoViewModel()
        {
            FUPermiss = new UserPermiss();
        }
    }
}
