using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class Airport
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }

        [JsonPropertyName("airport")]
        public required string AirportCode { get; set; }
    }
}
