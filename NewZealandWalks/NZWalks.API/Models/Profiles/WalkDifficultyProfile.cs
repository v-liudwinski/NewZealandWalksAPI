using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Models.Profiles;

public class WalkDifficultyProfile : Profile
{
    public WalkDifficultyProfile()
    {
        CreateMap<WalkDifficulty, WalkDifficultyDTO>()
            .ReverseMap();
    }
}