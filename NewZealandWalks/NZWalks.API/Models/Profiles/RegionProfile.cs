using AutoMapper;

namespace NZWalks.API.Models.Profiles;

public class RegionProfile : Profile
{
    public RegionProfile()
    {
        CreateMap<Models.Domain.Region, Models.DTO.Region>()
            .ReverseMap();
    }
}