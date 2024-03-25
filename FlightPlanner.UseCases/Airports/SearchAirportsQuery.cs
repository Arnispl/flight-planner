using FlightPlanner.UseCases.Models;
using MediatR;


namespace FlightPlanner.UseCases.Airports
{
    public class SearchAirportsQuery : IRequest<ServiceResult>
    {
        public string SearchTerm { get; set; }

        public SearchAirportsQuery(string search)
        {
            SearchTerm = search;
        }
    }
}
