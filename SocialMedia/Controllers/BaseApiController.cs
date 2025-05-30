using Microsoft.AspNetCore.Mvc;
using SocialMedia.Helpers;

namespace SocialMedia.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
