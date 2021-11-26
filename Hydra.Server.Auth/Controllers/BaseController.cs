namespace Hydra.Server.Auth.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Authorize(AuthenticationSchemes = "Identity.Application")]
    public class BaseController : Controller
    {
    }
}
