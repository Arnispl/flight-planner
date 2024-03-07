using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupApiController : ControllerBase
    {
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            FlightStorage.Clear();
            return Ok();
        }
    }
}
