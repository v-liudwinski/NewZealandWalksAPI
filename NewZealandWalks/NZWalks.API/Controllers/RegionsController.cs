using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionRepository.GetAllAsync();
            var regionsDTO = _mapper.Map<List<Models.DTO.RegionDTO>>(regions);
            
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);

            if (region is null) return NotFound();

            var regionDTO = _mapper.Map<Models.DTO.RegionDTO>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            /*// Validation
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }*/
            
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            region = await _regionRepository.AddAsync(region);

            var regionDTO = _mapper.Map<Models.DTO.RegionDTO>(region);
            return CreatedAtAction(nameof(GetRegionAsync), new {Id = regionDTO.Id}, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var region = await _regionRepository.DeleteAsync(id);

            if (region is null) return NotFound();
            
            var regionDTO = _mapper.Map<Models.DTO.RegionDTO>(region);
            
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            /*// Validation
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }*/
            
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            region = await _regionRepository.UpdateAsync(id, region);
            
            if (region is null) return NotFound();

            var regionDTO = _mapper.Map<Models.DTO.RegionDTO>(region);

            return Ok(regionDTO);
        }

        #region PrivateRegions

        /*
        private bool ValidateAddRegionAsync(AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest is null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), 
                    $"Region data is required!");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), 
                    $"{nameof(addRegionRequest.Code)} field could not be white space or empty!");
            }
            
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), 
                    $"{nameof(addRegionRequest.Name)} field could not be white space or empty!");
            }
            
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), 
                    $"{nameof(addRegionRequest.Area)} field could not be less than or equal to zero!");
            }
            
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), 
                    $"{nameof(addRegionRequest.Population)} field could not be less than zero!");
            }

            return ModelState.ErrorCount == 0;
        }
        
        private bool ValidateUpdateRegionAsync(UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest is null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), 
                    $"Region data is required!");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), 
                    $"{nameof(updateRegionRequest.Code)} field could not be white space or empty!");
            }
            
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), 
                    $"{nameof(updateRegionRequest.Name)} field could not be white space or empty!");
            }
            
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), 
                    $"{nameof(updateRegionRequest.Area)} field could not be less than or equal to zero!");
            }
            
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), 
                    $"{nameof(updateRegionRequest.Population)} field could not be less than zero!");
            }

            return ModelState.ErrorCount == 0;
        }
        */
        
        #endregion
    }
}
