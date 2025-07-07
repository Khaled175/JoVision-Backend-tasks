using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Task46.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreeterController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Post([FromForm] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Hello anonymous";
            }
            else
            {
                return $"Hello {name}";
            }
        }
    }
}
