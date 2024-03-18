using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        private static readonly object _lock = new object();

        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public Flight? GetFullFlightById(int id)
        {
            return _context.Flights
                .Include(flight => flight.From)
                .Include(flight => flight.To)
                .SingleOrDefault(flight => flight.Id == id);
        }
               
        public Boolean AddFlight(Flight flight)
        {
            lock (_lock)
            {
                var exists = _context.Flights.Any(f =>
                    f.Carrier == flight.Carrier &&
                    f.From.AirportCode == flight.From.AirportCode &&
                    f.To.AirportCode == flight.To.AirportCode &&
                    f.DepartureTime == flight.DepartureTime &&
                    f.ArrivalTime == flight.ArrivalTime);

                if (!exists)
                {
                    _context.Flights.Add(flight);
                    _context.SaveChanges();
                    return true; 
                }
                else
                {
                    return false; 
                }
            }
        }

        public void DeleteFlight(int id)
        {
            lock (_lock)
            {
                var flight = GetFullFlightById(id);
                if (flight != null)
                {
                    _context.Flights.Remove(flight);
                    _context.SaveChanges();
                }
            }
        }

        public void ClearData()
        {
            var flights = _context.Flights.ToList();
            _context.Flights.RemoveRange(flights);

            var airports = _context.Airports.ToList();
            _context.Airports.RemoveRange(airports);

            _context.SaveChanges();
        }

        public async Task<List<Airport>> SearchAirports(string search)
        {
            var lowerCaseSearch = search.ToLowerInvariant().Trim();
            return await _context.Airports
                .Where(a => a.AirportCode.ToLower().Contains(lowerCaseSearch)
                            || a.City.ToLower().Contains(lowerCaseSearch)
                            || a.Country.ToLower().Contains(lowerCaseSearch))
                .ToListAsync();
        }

        public PageResult<Flight> SearchFlights(SearchFlightsRequest request)
        {
            var query = _context.Flights.Include(f => f.From).Include(f => f.To).AsQueryable();

            if (!string.IsNullOrEmpty(request.From))
            {
                query = query.Where(f => f.From.AirportCode.Contains(request.From));
            }

            if (!string.IsNullOrEmpty(request.To))
            {
                query = query.Where(f => f.To.AirportCode.Contains(request.To));
            }

            if (request.Date.HasValue)
            {
                var date = request.Date.Value.Date;
                query = query.Where(f => DateTime.Parse(f.DepartureTime).Date == date);
            }

            var flights = query.ToList();

            return new PageResult<Flight>
            {
                Page = 0,
                TotalItems = flights.Count,
                Items = flights
            };
        }
    }
}
