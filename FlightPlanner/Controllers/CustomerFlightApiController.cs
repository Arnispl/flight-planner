using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;


namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerFlightApiController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IValidator<SearchFlightsRequest> _validator;


        public CustomerFlightApiController(
            IFlightService flightService, 
            IMapper mapper,
            IValidator<SearchFlightsRequest> validator)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validator = validator;
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
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
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


