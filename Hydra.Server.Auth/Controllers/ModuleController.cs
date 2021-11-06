using Microsoft.AspNetCore.Authorization;

namespace Hydra.Server.Auth.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ModuleController : Controller
    {
        public IActionResult Video()
        {
            return View();
        }
    }
}
