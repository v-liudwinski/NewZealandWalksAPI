using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories;

public class WalkDifficultyRepository : IWalkDifficultyRepository
{
    private readonly NZWalksDbContext _dbContext;

    public WalkDifficultyRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<WalkDifficulty>> GetAllWalkDifficultiesAsync()
    {
        return await _dbContext.WalkDifficulty
            .ToListAsync();
    }

    public async Task<WalkDifficulty> GetWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);
        return walkDifficulty!;
    }

    public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walkDifficulty)
    {
        walkDifficulty.Id = Guid.NewGuid();
        await _dbContext.WalkDifficulty.AddAsync(walkDifficulty);
        await _dbContext.SaveChangesAsync();
        return walkDifficulty;
    }

    public async Task<WalkDifficulty> UpdateWalkDifficultyAsync(Guid id, WalkDifficulty walkDifficulty)
    {
        var existingWalkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

        if (existingWalkDifficulty is null) return null!;

        existingWalkDifficulty.Code = walkDifficulty.Code;
        await _dbContext.SaveChangesAsync();

        return existingWalkDifficulty;
    }

    public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

        if (walkDifficulty is null) return null!;

        _dbContext.WalkDifficulty.Remove(walkDifficulty);
        await _dbContext.SaveChangesAsync();
        return walkDifficulty;
    }
}