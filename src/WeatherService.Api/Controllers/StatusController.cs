using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult GetStatus() => Ok();
    }
}