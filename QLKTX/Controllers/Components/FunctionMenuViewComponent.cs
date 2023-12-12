using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Entities;
using QLKTX.Class.Repository;
using QLKTX.Class.ViewModels;
using QLKTX.Models;

namespace QLKTX.Controllers.Components
{
    public class FunctionMenuViewComponent : ViewComponent
    {
        private readonly IMenuRepository _menuRepository;
        public FunctionMenuViewComponent(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public IViewComponentResult Invoke()
        {
            var moduleIDL = HttpContext.Request.Query["ModuleID"];
            var functionIDL = HttpContext.Request.Query["FunctionID"];
            if (moduleIDL.Count > 0)
            {
                int moduleID = int.Parse(moduleIDL[0]);
                var funcMenu = new FunctionMenuViewModel();
                funcMenu.FunctionID = int.Parse(functionIDL[0]);
                var funcS = HttpContext.Session.GetString(SystemConstants.FunctionMenuSession + "_" + moduleID);
                if (funcS != null)
                {
                    funcMenu.FunctionMenus = JsonConvert.DeserializeObject<List<FunctionMenuVm>>(funcS);
                }
                else
                {
                    var result = _menuRepository.GetFunctionMenu(moduleID);
                    if (result.Success)
                    {
                        funcMenu.FunctionMenus = (List<FunctionMenuVm>)result.Data.Items;
                        HttpContext.Session.SetString(SystemConstants.FunctionMenuSession + "_" + moduleID, JsonConvert.SerializeObject(funcMenu.FunctionMenus));
                    }
                }
                return View(funcMenu);
            }
            return View();
        }
    }
}
