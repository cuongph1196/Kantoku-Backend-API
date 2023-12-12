using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Class;
using QLKTX.Models;

namespace QLKTX.Controllers.Components
{
    public class ModuleMenuViewComponent: ViewComponent
    {
        private readonly IMenuRepository _menuRepository;
        public ModuleMenuViewComponent(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public IViewComponentResult Invoke()
        {
            var moduleMenu = new FunctionMenuViewModel();
            var modules = HttpContext.Session.GetString(SystemConstants.ModuleMenuSession);
            if (modules != null)
            {
                moduleMenu.ModuleMenus = JsonConvert.DeserializeObject<List<ModuleMenuVm>>(modules);
            }
            else
            {
                var result = _menuRepository.GetModuleMenu();
                if (result.Success)
                {
                    moduleMenu.ModuleMenus = (List<ModuleMenuVm>)result.Data.Items;
                    HttpContext.Session.SetString(SystemConstants.ModuleMenuSession, JsonConvert.SerializeObject(moduleMenu.ModuleMenus));
                }
            }
            return View(moduleMenu);
        }
    }
}
