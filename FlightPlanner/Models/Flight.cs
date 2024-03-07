namespace FlightPlanner.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public required Airport From { get; set; }
        public required Airport To { get; set; }
        public required string Carrier { get; set; }
        public required string DepartureTime { get; set; }
        public required string ArrivalTime { get; set; }
    }
}
