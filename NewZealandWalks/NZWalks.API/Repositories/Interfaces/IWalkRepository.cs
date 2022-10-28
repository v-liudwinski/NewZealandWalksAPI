using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Interfaces;

public interface IWalkRepository
{
    Task<IEnumerable<Walk>> GetAllAsync();
}