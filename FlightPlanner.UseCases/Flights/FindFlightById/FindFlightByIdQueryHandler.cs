using AutoMapper;
using FlightPlanner.Core.Services;
using FlightPlanner.UseCases.Flights;
using FlightPlanner.UseCases.Models;
using MediatR;
using System.Net;

public class FindFlightByIdQueryHandler : IRequestHandler<FindFlightByIdQuery, ServiceResult>
{
    private readonly IFlightService _flightService;
    private readonly IMapper _mapper;

    public FindFlightByIdQueryHandler(IFlightService flightService, IMapper mapper)
    {
        _flightService = flightService;
        _mapper = mapper;
    }

    public Task<ServiceResult> Handle(FindFlightByIdQuery request, CancellationToken cancellationToken)
    {
        var flight = _flightService.GetFullFlightById(request.Id); 
        if (flight == null)
        {
            return Task.FromResult(new ServiceResult
            {
                Status = HttpStatusCode.NotFound
            });
        }

        var flightResponse = _mapper.Map<FlightResponse>(flight);
        return Task.FromResult(new ServiceResult
        {
            Status = HttpStatusCode.OK,
            ResultObject = flightResponse
        });
    }
}


