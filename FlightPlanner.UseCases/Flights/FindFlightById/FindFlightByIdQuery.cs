using MediatR;
using FlightPlanner.UseCases.Models;

namespace FlightPlanner.UseCases.Flights
{
    public class FindFlightByIdQuery : IRequest<ServiceResult>
    {
        public int Id { get; }

        public FindFlightByIdQuery(int id)
        {
            Id = id;
        }
    }
}

