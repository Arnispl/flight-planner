using FlightPlanner.UseCases.Models;
using MediatR;


namespace FlightPlanner.UseCases.Flights.GetFlight
{
    public class GetFlightQuery(int id) : IRequest<ServiceResult>
    {
        public int Id { get; } = id;
    }
}
