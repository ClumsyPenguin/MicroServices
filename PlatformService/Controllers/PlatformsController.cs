using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient)
        : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetAllPlatforms()
        {
        #if DEBUG
            Console.WriteLine("Getting platforms...");
        #endif
            var platformItem = repository.GetAllPlatforms();

            if (!platformItem.Any())
            {
                return NotFound();
            }                
            return Ok(mapper.Map<IEnumerable<PlatformReadDTO>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            
            var platformItem = repository.GetPlatformById(id);

            if (platformItem is null)
            {
                return NotFound();
            }
            
            return Ok(mapper.Map<PlatformReadDTO>(platformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformCreateDto)
        {
            if (!ModelState.IsValid || platformCreateDto is null)
            {
                return BadRequest();
            }
            
            var platformModel = mapper.Map<Platform>(platformCreateDto);
            repository.CreatePlatform(platformModel);
            repository.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDTO>(platformModel);

            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex) 
            {
                //sync message exception
                #if DEBUG
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
                #endif
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { platformReadDto.Id }, platformReadDto);
        }
    }
}