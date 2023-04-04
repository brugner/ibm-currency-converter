using AutoMapper;
using IBM.Application.Models.DTOs.Rates;
using IBM.Domain.Entities;

namespace IBM.Application.MappingProfiles;

public class RateMappingProfile : Profile
{
    public RateMappingProfile()
    {
        CreateMap<ExternalRateDTO, Rate>()
            .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Rate));

        CreateMap<Rate, RateDTO>()
             .ForMember(dest => dest.Rate, options => options.MapFrom(src => src.Value));
    }
}