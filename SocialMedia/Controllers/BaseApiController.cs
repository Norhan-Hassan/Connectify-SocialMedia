using Microsoft.AspNetCore.Mvc;
using SocialMedia.Helpers;

namespace SocialMedia.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
