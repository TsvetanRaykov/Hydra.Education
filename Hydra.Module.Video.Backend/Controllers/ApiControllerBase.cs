using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Module.Video.Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/video/[controller]")]
    public class ApiControllerBase : ControllerBase
    {

    }
}