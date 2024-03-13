using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
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
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private static readonly object _globalLock = new object();

        public AdminApiController(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFullFlightById(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AddFlightResponse>(flight));
        }

        [HttpPut]
        [Route("flights")]
        public IActionResult AddFlight(AddFlightRequest request)
        {
            Console.WriteLine($"Received flight: {JsonSerializer.Serialize(request)}");

            if (request == null || !IsFlightValid(request))
            {
                return BadRequest();
            }

            var flight = _mapper.Map<Flight>(request);
            lock (_globalLock)
            {
                if (_flightService.Exists(flight))
                {
                    return StatusCode(409);
                }
                _flightService.Create(flight);
            }

            return Created("", (_mapper.Map<AddFlightResponse>(flight)));
        }

        private bool IsFlightValid(AddFlightRequest request)
        {
            return !(string.IsNullOrEmpty(request.Carrier) ||
                     string.IsNullOrEmpty(request.From?.Airport) ||
                     string.IsNullOrEmpty(request.From?.Country) ||
                     string.IsNullOrEmpty(request.From?.City) ||
                     string.IsNullOrEmpty(request.To?.Airport) ||
                     string.IsNullOrEmpty(request.To?.Country) ||
                     string.IsNullOrEmpty(request.To?.City) ||
                     DateTime.Parse(request.DepartureTime) >= DateTime.Parse(request.ArrivalTime) ||
                     request.From.Airport.Trim().Equals(request.To.Airport.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_globalLock)
            {
                var flight = _flightService.GetFullFlightById(id);

                if (flight == null)
                {
                   return Ok();
                }
               _flightService.Delete(flight);
            }

            return Ok();
        }
    }
}




