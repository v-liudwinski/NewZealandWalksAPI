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
    private readonly IMapper _mapper;

    public WalksController(IWalkRepository walkRepository, IMapper mapper)
    {
        _walkRepository = walkRepository;
        _mapper = mapper;
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
}