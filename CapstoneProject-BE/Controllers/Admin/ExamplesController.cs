using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProject_BE.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamplesController : ControllerBase
    {
        [HttpGet]
        public string GetConfirmationToken(string UserId)
        {
            return UserId;
        }
    }
}
