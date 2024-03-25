using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.Models;
using MediatR;
using System.Net;

namespace FlightPlanner.UseCases.Airports
{
    public class SearchAirportsQueryHandler : IRequestHandler<SearchAirportsQuery, ServiceResult>
    {
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public SearchAirportsQueryHandler(IFlightService flightService, IMapper mapper)
        {
            _flightService = flightService;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(SearchAirportsQuery request, CancellationToken cancellationToken)
        {
            var matchingAirports = await _flightService.SearchAirports(request.SearchTerm);
            var airportViewModels = _mapper.Map<List<AirportViewModel>>(matchingAirports);

            return new ServiceResult
            {
                Status = HttpStatusCode.OK,
                ResultObject = airportViewModels
            };
        }
    }
}

