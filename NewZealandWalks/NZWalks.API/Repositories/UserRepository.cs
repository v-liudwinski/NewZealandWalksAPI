using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NZWalksDbContext _dbContext;

    public UserRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AuthenticateAsync(string username, string password)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x =>
                x.Username.ToLower() == username.ToLower() 
                && x.Password == password);

        if (user is not null)
        {
            var userRoles = await _dbContext.User_Roles
                .Where(x => x.UserId == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                user.Roles = new List<string>();
                foreach (var userRole in userRoles)
                {
                    var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
                    if (role is not null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }
        }

        user.Password = null;
        return user;
    }
}