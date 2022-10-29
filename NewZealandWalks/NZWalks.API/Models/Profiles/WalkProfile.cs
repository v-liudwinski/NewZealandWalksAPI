using AutoMapper;

namespace NZWalks.API.Models.Profiles;

public class WalkProfile : Profile
{
    public WalkProfile()
    {
        CreateMap<Models.Domain.Walk, Models.DTO.WalkDTO>()
            .ReverseMap();
    }
}