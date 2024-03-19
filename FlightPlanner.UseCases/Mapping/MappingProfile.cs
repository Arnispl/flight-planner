using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.UseCases.Models;

namespace FlightPlanner.UseCases.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Airport, AirportViewModel>()
                .ForMember(viewModel => viewModel.Airport, options => options
                .MapFrom(source => source.AirportCode));
            CreateMap<AirportViewModel, Airport>()
                .ForMember(destination => destination.AirportCode, options => options
                .MapFrom(source => source.Airport));
            CreateMap<AddFlightRequest, Flight>();
            CreateMap<Flight, AddFlightResponse>();
            CreateMap<Flight, FlightResponse>()
                .ForMember(destination => destination.From, options => options.MapFrom(source => source.From))
    .ForMember(destination => destination.To, opt => opt.MapFrom(source => source.To));

        }
    }
}
