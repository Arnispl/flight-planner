using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(Flight flight)
        {
            Console.WriteLine($"Received flight: {JsonSerializer.Serialize(flight)}");

            if (flight == null)
            {
                return BadRequest();
            }

            if (
                string.IsNullOrEmpty(flight.Carrier) ||
                string.IsNullOrEmpty(flight.From.AirportCode) ||
                string.IsNullOrEmpty(flight.From.Country) ||
                string.IsNullOrEmpty(flight.From.City) ||
                string.IsNullOrEmpty(flight.To.AirportCode) ||
                string.IsNullOrEmpty(flight.To.Country) ||
                string.IsNullOrEmpty(flight.To.City) ||
                string.IsNullOrEmpty(flight.To.Country)
               )
            {
                return BadRequest();
            }

            if (FlightStorage.FlightExists(flight))
            {
                return Conflict();
            }

            var fromAirport = flight.From.AirportCode.Trim();
            var toAirport = flight.To.AirportCode.Trim();

            if (string.Equals(fromAirport, toAirport, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            if (DateTime.Parse(flight.DepartureTime) >= DateTime.Parse(flight.ArrivalTime))
            {
                return BadRequest();
            }

            AirportStorage.AddAirport(flight.From);
            AirportStorage.AddAirport(flight.To);

            FlightStorage.AddFlight(flight);
            return Created("", flight);

        }
        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight != null)
            {
                FlightStorage.DeleteFlightById(id);
            }

            return Ok();
        }
    }
}
