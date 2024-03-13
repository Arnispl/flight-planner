using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight? GetFullFlightById(int id);
        bool Exists(Flight flight);
        void ClearData();
        Task<List<Airport>> SearchAirports(string search);
        PageResult<Flight> SearchFlights(SearchFlightsRequest request);
    }
}
