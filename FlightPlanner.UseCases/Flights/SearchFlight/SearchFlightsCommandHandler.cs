using MediatR;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.Models;
using FlightPlanner.Core.Models;
using FluentValidation;
using System.Net;

namespace FlightPlanner.UseCases.Flights
{
    public class SearchFlightsCommandHandler : IRequestHandler<SearchFlightsCommand, ServiceResult>
    {
        private readonly IFlightService _flightService;
        private readonly IValidator<SearchFlightsRequest> _validator;

        public SearchFlightsCommandHandler(IFlightService flightService, IValidator<SearchFlightsRequest> validator)
        {
            _flightService = flightService;
            _validator = validator;
        }

        public async Task<ServiceResult> Handle(SearchFlightsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request.Request);
            if (!validationResult.IsValid)
            {
                return new ServiceResult
                {
                    Status = HttpStatusCode.BadRequest,
                };
            }

            var result = _flightService.SearchFlights(request.Request);
            return new ServiceResult
            {
                Status = HttpStatusCode.OK,
                ResultObject = result
            };
        }
    }
}
