using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Interfaces;

public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetAllAsync();
}