namespace FlightPlanner.UseCases.Models
{
    public class FlightResponse
    {
        public int Id { get; set; }
        public AirportViewModel From { get; set; }
        public AirportViewModel To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
