﻿using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerFlightApiController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;

        public CustomerFlightApiController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("airports")]
        public async Task<IActionResult> SearchAirports([FromQuery] string search)
        {
            var lowerCaseSearch = search.ToLowerInvariant().Trim();
            var matchingAirports = await _context.Airports
                .Where(a => a.AirportCode.Contains(lowerCaseSearch)
                    || a.City.Contains(lowerCaseSearch)
                    || a.Country.Contains(lowerCaseSearch))
                 .ToListAsync();

            return Ok(matchingAirports);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            if (request == null ||
                (string.IsNullOrEmpty(request.From) && string.IsNullOrEmpty(request.To) && !request.Date.HasValue) ||
                (request.From == request.To))
            {
                return BadRequest();
            }

            var query = _context.Flights.Include(f => f.From).Include(f => f.To).AsQueryable();

            if (!string.IsNullOrEmpty(request.From))
            {
                query = query.Where(f => f.From.AirportCode.Contains(request.From));
            }

            if (!string.IsNullOrEmpty(request.To))
            {
                query = query.Where(f => f.To.AirportCode.Contains(request.To));
            }

            if (request.Date.HasValue)
            {
                var date = request.Date.Value.Date;

                query = query.Where(f => DateTime.Parse(f.DepartureTime).Date == date);
            }

            var flights = query.ToList();

            var result = new PageResult<Flight>
            {
                Page = 0,
                TotalItems = flights.Count,
                Items = flights
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _context.Flights
                .Include(f => f.From)
                .Include(f => f.To)
                .FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}


