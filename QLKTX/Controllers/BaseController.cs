using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLKTX.Class;
using QLKTX.Class.Entities;

namespace QLKTX.Controllers
{
    //[TypeFilter(typeof(SessionModelValidate))]
    public abstract class BaseController : Controller
    {
        public LoggedInUser LoggedInUser => HttpContext.Session.GetString(SystemConstants.UserSession) != null
                ? JsonConvert.DeserializeObject<LoggedInUser>(HttpContext.Session.GetString(SystemConstants.UserSession)) : null;
    }
}
