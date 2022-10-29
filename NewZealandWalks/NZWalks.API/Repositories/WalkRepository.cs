using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories;

public class WalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;
    
    public WalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Walk>> GetAllAsync()
    {
        return await _dbContext.Walks
            .Include(x => x.Region)
            .Include(x => x.WalkDifficulty)
            .ToListAsync();
    }

    public async Task<Walk> GetWalkAsync(Guid id)
    {
        return (await _dbContext.Walks
            .Include(x => x.Region)
            .Include(x => x.WalkDifficulty)
            .FirstOrDefaultAsync(x => x.Id == id))!;
    }

    public async Task<Walk> AddWalkAsync(Walk walk)
    {
        walk.Id = Guid.NewGuid();
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
    {
        var existingWalk = (await _dbContext.Walks
            .Include(x => x.Region)
            .Include(x => x.WalkDifficulty)
            .FirstOrDefaultAsync(x => x.Id == id))!;

        if (existingWalk is null) return null!;

        existingWalk.Name = walk.Name;
        existingWalk.Length = walk.Length;
        existingWalk.RegionId = walk.RegionId;
        existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
        existingWalk.Region = walk.Region;
        existingWalk.WalkDifficulty = walk.WalkDifficulty;

        await _dbContext.SaveChangesAsync();

        return existingWalk;
    }

    public async Task<Walk> DeleteWalkAsync(Guid id)
    {
        var walk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

        if (walk is null) return null!;

        _dbContext.Walks.Remove(walk);
        await _dbContext.SaveChangesAsync();

        return walk;
    }
}