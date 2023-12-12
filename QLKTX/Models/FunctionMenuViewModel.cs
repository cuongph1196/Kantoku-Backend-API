using QLKTX.Class.ViewModels;

namespace QLKTX.Models
{
    public class FunctionMenuViewModel
    {
        public int FunctionID { get; set; }
        public List<ModuleMenuVm> ModuleMenus { get; set; }
        public List<FunctionMenuVm> FunctionMenus { get; set; }
        public FunctionMenuViewModel()
        {
            ModuleMenus = new List<ModuleMenuVm>();
            FunctionMenus = new List<FunctionMenuVm>(); 
        }
    }

}
