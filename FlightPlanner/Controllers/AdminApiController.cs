using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        public readonly FlightPlannerDbContext _context;

        public AdminApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _context.Flights
                .Include(flight=>flight.To)
                .Include(flight => flight.From)
                .FirstOrDefault(flight => flight.Id == id);
            
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

            _context.Flights.Add(flight);
            _context.SaveChanges();

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
