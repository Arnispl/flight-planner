namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime? Date { get; set; }
    }
}
