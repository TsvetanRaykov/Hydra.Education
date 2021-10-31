using Microsoft.AspNetCore.Authorization;

namespace Hydra.Server.Auth.Controllers
{
    using Microsoft.AspNetCore.Mvc;

   
    public class ModuleController : Controller
    {
        //[Authorize(Roles = "Admin")]
        public IActionResult Video()
        {
            return View();
        }
    }
}
