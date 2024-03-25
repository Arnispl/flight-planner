using System.Net;

namespace FlightPlanner.UseCases.Models
{
    public class ServiceResult
    {
        public object ResultObject { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
