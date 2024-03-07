using FlightPlanner.Models;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Storage
{
    public static class AirportStorage
    {
        private static readonly Dictionary<string, Airport> _airports = new Dictionary<string, Airport>();
        private static readonly Dictionary<string, int> _airportUsage = new Dictionary<string, int>();
        private static readonly object _airportLock = new object();

        public static void AddAirport(Airport airport)
        {
            lock (_airportLock)
            {
                if (!_airports.ContainsKey(airport.AirportCode))
                {
                    _airports.Add(airport.AirportCode, airport);
                    _airportUsage[airport.AirportCode] = 1;
                }
                else
                {
                    _airportUsage[airport.AirportCode]++;
                }
            }
        }

        public static void RemoveAirportUsage(string airportCode)
        {
            lock (_airportLock)
            {
                if (_airportUsage.TryGetValue(airportCode, out var usage))
                {
                    if (usage <= 1)
                    {
                        _airportUsage.Remove(airportCode);
                        _airports.Remove(airportCode);
                    }
                    else
                    {
                        _airportUsage[airportCode] = usage - 1;
                    }
                }
            }
        }

        public static List<Airport> SearchAirports(string phrase)
        {
            phrase = phrase?.Trim().ToLower();

            List<Airport> airportsList;
            lock (_airportLock)
            {
                airportsList = _airports.Values.Where(a =>
                    a.City.ToLower().Contains(phrase) ||
                    a.Country.ToLower().Contains(phrase) ||
                    a.AirportCode.ToLower().Contains(phrase))
                    .ToList();
            }

            return airportsList;
        }
    }
}


