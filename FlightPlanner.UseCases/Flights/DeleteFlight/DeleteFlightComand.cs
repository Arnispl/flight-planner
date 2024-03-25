using FlightPlanner.UseCases.Models;
using MediatR;

namespace FlightPlanner.UseCases.Flights.DeleteFlight
{
    public class DeleteFlightComand : IRequest<ServiceResult>
    {
        public int Id { get; set; } 
    }
}
