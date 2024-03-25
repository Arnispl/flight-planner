using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight? GetFullFlightById(int id);
        Boolean AddFlight(Flight flight);
        void DeleteFlight(int id);
        Task<List<Airport>> SearchAirports(string search);
        PageResult<Flight> SearchFlights(SearchFlightsRequest request);
    }
}
