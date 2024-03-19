using FlightPlanner.UseCases.Models;
using MediatR;

namespace FlightPlanner.UseCases.Flights.AddFlight
{
    public class AddFlightComand : IRequest<ServiceResult>
    {
        public AddFlightRequest AddFlightRequest { get; set; }
    }
}
