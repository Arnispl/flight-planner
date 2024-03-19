using MediatR;
using FlightPlanner.UseCases.Models;
using FlightPlanner.Core.Models;

namespace FlightPlanner.UseCases.Flights
{
    public class SearchFlightsCommand : IRequest<ServiceResult>
    {
        public SearchFlightsRequest Request { get; }

        public SearchFlightsCommand(SearchFlightsRequest request)
        {
            Request = request;
        }
    }
}