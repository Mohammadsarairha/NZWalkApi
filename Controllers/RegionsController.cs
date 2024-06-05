using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();

            var regionsDto = mapper.Map<List<RegionDto>>(regions);

            //var regionsDto = new List<RegionDto>();
            //foreach (var item in regions)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = item.Id,
            //        Code = item.Code,
            //        Name = item.Name,
            //        RegionImageUrl = item.RegionImageUrl
            //    });
            //}
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);

            var region = await regionRepository.GetById(id);

            if (region == null)
                return NotFound();

            //var regionDto = new RegionDto
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl
            //};

            return Ok(mapper.Map<RegionDto>(region));
        }

        //Create Region
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequest)
        {
            if (ModelState.IsValid)
            {
                var region = mapper.Map<Region>(addRegionRequest);

                await regionRepository.Create(region);

                var regionDto = mapper.Map<RegionDto>(region);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        // id in Route should be same in function parameter to can map to it
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequest)
        {
            if (ModelState.IsValid)
            {
                var dominRegion = mapper.Map<Region>(updateRegionRequest);
                var region = await regionRepository.Update(id, dominRegion);

                if (region == null)
                    return NotFound();

                return Ok(mapper.Map<RegionDto>(region));
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.Delete(id);

            if (region == null)
                return NotFound(); // Returns a 404 Not Found

            return NoContent(); // Returns a 204 No Content
        }
    }
}

