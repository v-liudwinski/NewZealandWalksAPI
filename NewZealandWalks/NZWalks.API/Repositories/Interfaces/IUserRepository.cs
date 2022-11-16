using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AuthenticateAsync(string username, string password);
}