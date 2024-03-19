using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.Models;
using MediatR;
using System.Net;

namespace FlightPlanner.UseCases.Flights.DeleteFlight
{
    public class DeleteFlightComandHandler : IRequestHandler<DeleteFlightComand, ServiceResult>
    {
        private readonly IFlightService _flightService;

        public DeleteFlightComandHandler(IFlightService flightService)
        {
            _flightService = flightService;
        }

        public Task<ServiceResult> Handle(DeleteFlightComand request, CancellationToken cancellationToken)
        {
            _flightService.DeleteFlight(request.Id);

            var response = new ServiceResult
            {
                Status = HttpStatusCode.OK,
                ResultObject = new { Message = "Flight deleted successfully" }
            };

            return Task.FromResult(response);
        }
    }
}
