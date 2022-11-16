using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Controllers;

[Route("[controller]")]
[ApiController]
public class WalkDifficultiesController : ControllerBase
{
    private readonly IWalkDifficultyRepository _walkDifficultyRepository;
    private readonly IMapper _mapper;

    public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
    {
        _walkDifficultyRepository = walkDifficultyRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "reader")]
    public async Task<IActionResult> GetAllWalkDifficultiesAsync()
    {
        var walkDifficulties = await _walkDifficultyRepository
            .GetAllWalkDifficultiesAsync();

        var walkDifficultiesDTO = _mapper.Map<List<WalkDifficultyDTO>>(walkDifficulties);

        return Ok(walkDifficultiesDTO);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetWalkDifficultyAsync")]
    [Authorize(Roles = "reader")]
    public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficultyAsync(id);

        if (walkDifficulty is null) return NotFound();
        
        var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);
        return Ok(walkDifficultyDTO);
    }

    [HttpPost]
    [Authorize(Roles = "writer")]
    public async Task<IActionResult> AddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
    {
        /*// Validation
        if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
        {
            return BadRequest(ModelState);
        }*/
        
        var walkDifficulty = new WalkDifficulty()
        {
            Code = addWalkDifficultyRequest.Code
        };
        
        walkDifficulty = await _walkDifficultyRepository.AddWalkDifficultyAsync(walkDifficulty);
        var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);
        
        return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id},
            walkDifficultyDTO);
    }

    [HttpPut]
    [Route("{id:guid}")]
    [Authorize(Roles = "writer")]
    public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, 
        UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
    {
        /*// Validation
        if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
        {
            return BadRequest(ModelState);
        }*/
        
        var walkDifficulty = new WalkDifficulty()
        {
            Code = updateWalkDifficultyRequest.Code
        };
        
        walkDifficulty = await _walkDifficultyRepository.UpdateWalkDifficultyAsync(id, walkDifficulty);

        if (walkDifficulty is null) return NotFound();
        
        var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);

        return Ok(walkDifficultyDTO);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = "writer")]
    public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);

        if (walkDifficulty is null) return NotFound();

        var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);
        return Ok(walkDifficultyDTO);
    }

    #region Private Methods
    
    /*
    private bool ValidateAddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
    {
        if (addWalkDifficultyRequest is null)
        {
            ModelState.AddModelError(nameof(addWalkDifficultyRequest), 
                $"Walk Difficulty is required!");
        }

        if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
        {
            ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), 
                $"{nameof(addWalkDifficultyRequest.Code)} could not be empty!");
        }
        
        return ModelState.ErrorCount == 0;
    }
    
    private bool ValidateUpdateWalkDifficultyAsync(UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
    {
        if (updateWalkDifficultyRequest is null)
        {
            ModelState.AddModelError(nameof(updateWalkDifficultyRequest), 
                $"Walk Difficulty is required!");
        }

        if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
        {
            ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), 
                $"{nameof(updateWalkDifficultyRequest.Code)} could not be empty!");
        }
        
        return ModelState.ErrorCount == 0;
    }
    */

    #endregion
}