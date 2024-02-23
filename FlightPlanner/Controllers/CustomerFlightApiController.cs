using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerFlightApiController : ControllerBase
    {
        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports([FromQuery] string search)
        {
            var matchingAirports = AirportStorage.SearchAirports(search);

            if (!matchingAirports.Any())
            {
                return NotFound();
            }

            return Ok(matchingAirports);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            if (request == null ||
                (string.IsNullOrEmpty(request.From) && string.IsNullOrEmpty(request.To) && request.Date == null) ||
                (request.From == request.To))
            {
                return BadRequest();
            }

            var flights = FlightStorage.SearchFlights(request);

            var result = new PageResult<Flight>
            {
                Page = 0,
                TotalItems = flights?.Count ?? 0,
                Items = flights ?? new List<Flight>()
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
