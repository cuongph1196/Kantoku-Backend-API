using Microsoft.AspNetCore.Mvc;
using QLKTX.Class.Entities;

namespace QLKTX.Controllers
{
    public abstract class BaseMController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public LoggedInUser LoggedInUserMember => (LoggedInUser)HttpContext.Items["UserLogin"];
    }
}
