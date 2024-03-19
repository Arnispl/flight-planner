namespace FlightPlanner.UseCases.Models
{
    public class AddFlightRequest
    {
        public required AirportViewModel From { get; set; }
        public required AirportViewModel To { get; set; }
        public required string Carrier { get; set; }
        public required string DepartureTime { get; set; }
        public required string ArrivalTime { get; set; }
    }
}
