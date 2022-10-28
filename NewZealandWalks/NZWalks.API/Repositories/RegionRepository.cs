using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    public RegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Region>> GetAllAsync()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region> GetAsync(Guid id)
    {
        return (await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id))!;
    }

    public async Task<Region> AddAsync(Region region)
    {
        region.Id = Guid.NewGuid();
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region> DeleteAsync(Guid id)
    {
        var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        
        if (region == null) return null!;

        _dbContext.Regions.Remove(region);
        await _dbContext.SaveChangesAsync();

        return region;
    }

    public async Task<Region> UpdateAsync(Guid id, Region region)
    {
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (existingRegion is null) return null!;

        existingRegion.Code = region.Code;
        existingRegion.Name = region.Name;
        existingRegion.Area = region.Area;
        existingRegion.Lat = region.Lat;
        existingRegion.Long = region.Long;
        existingRegion.Population = region.Population;

        await _dbContext.SaveChangesAsync();

        return existingRegion;
    }
}