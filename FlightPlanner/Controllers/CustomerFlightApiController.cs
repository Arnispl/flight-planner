using FlightPlanner.Core.Models;
using FlightPlanner.Extensions;
using FlightPlanner.UseCases.Airports;
using FlightPlanner.UseCases.Flights;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerFlightApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerFlightApiController(IMediator mediator)
        {          
            _mediator = mediator;
        }

        [HttpGet]
        [Route("airports")]
        public async Task<IActionResult> SearchAirports([FromQuery] string search)
        {
            return (await _mediator.Send(new SearchAirportsQuery(search)))
                .ToActionResult();
        }

        [HttpPost]
        [Route("flights/search")]
        public async Task<IActionResult> SearchFlights([FromBody] SearchFlightsRequest request)
        {
            return (await _mediator.Send(new SearchFlightsCommand(request)))
                .ToActionResult();
        }
                
         [HttpGet]
         [Route("flights/{id}")]
         public async Task<IActionResult> FindFlightById(int id)
         {
            return (await _mediator.Send(new FindFlightByIdQuery(id)))
                .ToActionResult();
         }
    }
}



