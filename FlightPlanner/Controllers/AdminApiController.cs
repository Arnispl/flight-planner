using FlightPlanner.Models;
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
        private static readonly object _globalLock = new object();

        public AdminApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _context.Flights
                .Include(flight => flight.To)
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

            if (flight == null || !IsFlightValid(flight))
            {
                return BadRequest();
            }

            DateTime departureTime = DateTime.Parse(flight.DepartureTime);
            DateTime arrivalTime = DateTime.Parse(flight.ArrivalTime);

            if (departureTime >= arrivalTime)
            {
                return BadRequest();
            }

            if (string.Equals(flight.From.AirportCode.Trim(), flight.To.AirportCode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            lock (_globalLock)
            {
                if (_context.Flights.Any(f => f.Carrier == flight.Carrier &&
                                          f.From.AirportCode == flight.From.AirportCode &&
                                          f.To.AirportCode == flight.To.AirportCode &&
                                          f.DepartureTime == flight.DepartureTime &&
                                          f.ArrivalTime == flight.ArrivalTime))
                {
                    return StatusCode(409);
                }

                _context.Flights.Add(flight);
                _context.SaveChanges();
            }

            return Created("", flight);
        }


        private bool IsFlightValid(Flight flight)
        {
            return !(string.IsNullOrEmpty(flight.Carrier) ||
                     string.IsNullOrEmpty(flight.From?.AirportCode) ||
                     string.IsNullOrEmpty(flight.From?.Country) ||
                     string.IsNullOrEmpty(flight.From?.City) ||
                     string.IsNullOrEmpty(flight.To?.AirportCode) ||
                     string.IsNullOrEmpty(flight.To?.Country) ||
                     string.IsNullOrEmpty(flight.To?.City));
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _context.Flights.FirstOrDefault(f => f.Id == id);

            if (flight != null)
            {
                _context.Flights.Remove(flight);
                _context.SaveChanges(); 
            }

            return Ok();
        }
    }
}


