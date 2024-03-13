using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerFlightApiController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public CustomerFlightApiController(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("airports")]
        public async Task<IActionResult> SearchAirports([FromQuery] string search)
        {
            var matchingAirports = await _flightService.SearchAirports(search);

            var airportResponses = _mapper.Map<List<AirportViewModel>>(matchingAirports);

            return Ok(airportResponses);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            if (request.From == request.To)
            {
                return BadRequest();
            }

            var result = _flightService.SearchFlights(request);

            if (result.Items.Count == 0)
            {                
                return Ok(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _flightService.GetFullFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            var flightResponse = _mapper.Map<FlightResponse>(flight);

            return Ok(flightResponse);
        }
    }
}


