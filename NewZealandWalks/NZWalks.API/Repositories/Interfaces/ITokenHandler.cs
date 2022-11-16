using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Interfaces;

public interface ITokenHandler
{
    Task<string> CreateTokenAsync(User user);
}