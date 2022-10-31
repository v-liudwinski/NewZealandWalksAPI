using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Controllers;

[Route("[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IWalkRepository _walkRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IWalkDifficultyRepository _walkDifficultyRepository;
    private readonly IMapper _mapper;

    public WalksController(IWalkRepository walkRepository, IMapper mapper, 
        IWalkDifficultyRepository walkDifficultyRepository, IRegionRepository regionRepository)
    {
        _walkRepository = walkRepository;
        _mapper = mapper;
        _walkDifficultyRepository = walkDifficultyRepository;
        _regionRepository = regionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWalksAsync()
    {
        var walks = await _walkRepository.GetAllAsync();
        
        var walksDTO = _mapper.Map<List<Models.DTO.WalkDTO>>(walks);
        
        return Ok(walksDTO);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetWalkAsync")]
    public async Task<IActionResult> GetWalkAsync(Guid id)
    {
        var walk = await _walkRepository.GetWalkAsync(id);

        if (walk is null) return NotFound();

        var walkDTO = _mapper.Map<WalkDTO>(walk);
        
        return Ok(walkDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
    {
        // Validation
        if (!(await ValidateAddWalkAsync(addWalkRequest)))
        {
            return BadRequest(ModelState);
        }
        
        var walk = new Walk()
        {
            Name = addWalkRequest.Name,
            Length = addWalkRequest.Length,
            RegionId = addWalkRequest.RegionId,
            WalkDifficultyId = addWalkRequest.WalkDifficultyId
        };

        walk = await _walkRepository.AddWalkAsync(walk);

        var walkDTO = _mapper.Map<WalkDTO>(walk);

        return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, 
        [FromBody] UpdateWalkRequest updateWalkRequest)
    {
        // Validation
        if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
        {
            return BadRequest(ModelState);
        }
        
        var walk = new Walk()
        {
            Name = updateWalkRequest.Name,
            Length = updateWalkRequest.Length,
            RegionId = updateWalkRequest.RegionId,
            WalkDifficultyId = updateWalkRequest.WalkDifficultyId
        };

        walk = await _walkRepository.UpdateWalkAsync(id, walk);

        if (walk is null) return NotFound();

        var walkDTO = _mapper.Map<WalkDTO>(walk);

        return Ok(walkDTO);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteWalkAsync(Guid id)
    {
        var walk = await _walkRepository.DeleteWalkAsync(id);

        if (walk is null) return NotFound();

        var walkDTO = _mapper.Map<WalkDTO>(walk);

        return Ok(walkDTO);
    }

    #region Private Methods

    private async Task<bool> ValidateAddWalkAsync(AddWalkRequest addWalkRequest)
    {
        if (addWalkRequest == null)
        {
            ModelState.AddModelError(nameof(addWalkRequest), 
                $"Walk is required!");
        }

        if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
        {
            ModelState.AddModelError(nameof(addWalkRequest.Name), 
                $"{nameof(addWalkRequest.Name)} could not be empty!");
        }

        if (addWalkRequest.Length <= 0)
        {
            ModelState.AddModelError(nameof(addWalkRequest.Length), 
                $"{nameof(addWalkRequest.Length)} could not be less than or equal to zero!");
        }

        var region = await _regionRepository.GetAsync(addWalkRequest.RegionId);
        if (region is null)
        {
            ModelState.AddModelError(nameof(addWalkRequest.RegionId), 
                $"{nameof(addWalkRequest.RegionId)} is not correct, looks like such region does not exist!");
        }

        var walkDifficulty = await _walkDifficultyRepository
            .GetWalkDifficultyAsync(addWalkRequest.WalkDifficultyId);
        if (walkDifficulty is null)
        {
            ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), 
                $"{nameof(addWalkRequest.WalkDifficultyId)} is not correct, " +
                $"looks like such walk difficulty does not exist!");
        }
        
        return ModelState.ErrorCount == 0;
    }
    
    private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequest updateWalkRequest)
    {
        if (updateWalkRequest == null)
        {
            ModelState.AddModelError(nameof(updateWalkRequest), 
                $"Walk is required!");
        }

        if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
        {
            ModelState.AddModelError(nameof(updateWalkRequest.Name), 
                $"{nameof(updateWalkRequest.Name)} could not be empty!");
        }

        if (updateWalkRequest.Length <= 0)
        {
            ModelState.AddModelError(nameof(updateWalkRequest.Length), 
                $"{nameof(updateWalkRequest.Length)} could not be less than or equal to zero!");
        }

        var region = await _regionRepository.GetAsync(updateWalkRequest.RegionId);
        if (region is null)
        {
            ModelState.AddModelError(nameof(updateWalkRequest.RegionId), 
                $"{nameof(updateWalkRequest.RegionId)} is not correct, looks like such region does not exist!");
        }

        var walkDifficulty = await _walkDifficultyRepository
            .GetWalkDifficultyAsync(updateWalkRequest.WalkDifficultyId);
        if (walkDifficulty is null)
        {
            ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), 
                $"{nameof(updateWalkRequest.WalkDifficultyId)} is not correct, " +
                $"looks like such walk difficulty does not exist!");
        }
        
        return ModelState.ErrorCount == 0;
    }

    
    #endregion
}