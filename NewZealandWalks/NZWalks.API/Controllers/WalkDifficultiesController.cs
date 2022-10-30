using AutoMapper;
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
    public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficultyAsync(id);

        if (walkDifficulty is null) return NotFound();
        
        var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);
        return Ok(walkDifficultyDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
    {
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
    public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, 
        UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
    {
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
    public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);

        if (walkDifficulty is null) return NotFound();

        var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);
        return Ok(walkDifficultyDTO);
    }
}