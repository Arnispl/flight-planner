using FlightPlanner.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FlightPlanner.Data
{
    public interface IFlightPlannerDbContext
    {
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        DbSet<Airport> Airports { get; set; }
        DbSet<Flight> Flights { get; set; }
        int SaveChanges(); 
    }
}
