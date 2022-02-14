namespace AstroCue.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class PlaceholderController : Controller
    {
        [HttpGet]
        [Route("placeholder")]
        public IActionResult Index()
        {
            return this.StatusCode(200, new
            {
                message = "Hello world"
            });
        }
    }
}
