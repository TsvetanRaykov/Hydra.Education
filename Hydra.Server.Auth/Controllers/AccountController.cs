namespace Hydra.Server.Auth.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Mvc;

    //[tr]: 2021-10-26
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("Login")]
        public ActionResult Login(string returnUrl = "/")
        {
            if (Url.IsLocalUrl(returnUrl)) return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
            ModelState.AddModelError(nameof(returnUrl), "Value must be a local URL");
            return BadRequest(ModelState);
        }

        [HttpGet("Logout")]
        public IActionResult Logout() => SignOut(
            new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
