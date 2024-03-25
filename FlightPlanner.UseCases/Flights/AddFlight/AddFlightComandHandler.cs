using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.Models;
using FluentValidation;
using MediatR;
using FlightPlanner.Services;
using System.Net;

namespace FlightPlanner.UseCases.Flights.AddFlight
{
    public class AddFlightComandHandler : IRequestHandler<AddFlightComand, ServiceResult>
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddFlightRequest> _validator;

        public AddFlightComandHandler(
            IFlightService flightService, 
            IMapper mapper, 
            IValidator<AddFlightRequest> validator)
        {
            _flightService = flightService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ServiceResult> Handle(AddFlightComand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.AddFlightRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ServiceResult
                {
                    ResultObject = validationResult.Errors,
                    Status = HttpStatusCode.BadRequest
                };
            }

            var flight = _mapper.Map<Flight>(request.AddFlightRequest);
            var addFlightResult = _flightService.AddFlight(flight);
            if (!addFlightResult)
            {
                return new ServiceResult
                {
                    Status = HttpStatusCode.Conflict
                };
            }

            return new ServiceResult
            {
                ResultObject = _mapper.Map<AddFlightResponse>(flight),
                Status = HttpStatusCode.Created
            };           
        }
    }
}
