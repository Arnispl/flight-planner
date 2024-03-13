using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Models;

namespace FlightPlanner.Mapping
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
        }
    }
}
