using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //POST : /api/Walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            //Map DTO to Domain
            var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);

            //Map Domain to Dto

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Get : /api/Walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walks = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            return Ok(mapper.Map<List<WalkDto>>(walks));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
                return NotFound();

            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto walkRequestDto)
        {
            // Map updateDto to Domain

            var walkDomain = mapper.Map<Walk>(walkRequestDto);

            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
                return NotFound();

            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);
            if (walk == null)
                return NotFound();

            return NoContent();
        }
    }
}

