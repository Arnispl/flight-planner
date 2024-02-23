using FlightPlanner.Models;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static readonly Dictionary<int, Flight> _flights = new Dictionary<int, Flight>();
        private static int _id = 1;
        private static readonly object _lock = new object();

        public static void AddFlight(Flight flight)
        {
            lock (_lock)
            {
                if (!FlightExists(flight))
                {
                    flight.Id = Interlocked.Increment(ref _id);
                    _flights[flight.Id] = flight;
                }
            }
        }

        public static bool FlightExists(Flight flight)
        {
            lock (_lock)
            {
                return _flights.Values.Any(f => f.Carrier == flight.Carrier &&
                                                f.From.AirportCode == flight.From.AirportCode &&
                                                f.To.AirportCode == flight.To.AirportCode &&
                                                f.DepartureTime == flight.DepartureTime &&
                                                f.ArrivalTime == flight.ArrivalTime);
            }
        }

        public static Flight? GetFlightById(int id)
        {
            lock (_lock)
            {
                _flights.TryGetValue(id, out var flight);
                return flight;
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _flights.Clear();
            }
        }

        public static void DeleteFlightById(int id)
        {
            lock (_lock)
            {
                if (_flights.ContainsKey(id))
                {
                    var removedFlight = _flights[id];
                    _flights.Remove(id);
                    AirportStorage.RemoveAirportUsage(removedFlight.From.AirportCode);
                    AirportStorage.RemoveAirportUsage(removedFlight.To.AirportCode);
                }
            }
        }

        public static List<Flight> SearchFlights(SearchFlightsRequest request)
        {
            lock (_lock)
            {
                return _flights.Values.Where(f =>
                {
                    var departureDate = DateTime.Parse(f.DepartureTime);
                    return (string.IsNullOrEmpty(request.From) || f.From.AirportCode == request.From) &&
                           (string.IsNullOrEmpty(request.To) || f.To.AirportCode == request.To) &&
                           (!request.Date.HasValue || departureDate.Date == request.Date.Value.Date);
                }).ToList();
            }
        }
    }
}
