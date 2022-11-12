using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System.Diagnostics;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : Controller
    {
        private readonly IPlatformRepo platformRepo;
        private readonly IMapper mapper;
        public ICommandDataClient CommandDataClient { get; }
        private IMessageBusClient MessageBusClient { get; }

        public PlatformsController(IPlatformRepo platformRepo
            , IMapper mapper
            , ICommandDataClient commandDataClient
            , IMessageBusClient messageBusClient )
        {
            this.platformRepo = platformRepo;
            this.mapper = mapper;
            CommandDataClient = commandDataClient;
            MessageBusClient = messageBusClient;
        }
        [HttpGet("GetPlatformsAsync", Name = "GetPlatformsAsync")]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatformsAsync()
        {
            var platforms = await platformRepo.GetAllFlatformsAsync();
            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("GetPlatformsByIdAsync", Name = "GetPlatformsByIdAsync")]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformsByIdAsync(int id)
        {
            if(id == 0)
                return BadRequest("Incorrect Id");

            var platforms = await platformRepo.GetFlatformByIdAsync(id);
            if(platforms == null)
                return NotFound();

            return Ok(mapper.Map<PlatformReadDto>(platforms));
        }

        [HttpPost("CreatePlatformsAsync", Name = "CreatePlatformsAsync")]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatformsAsync(PlatformCreateDto platform)
        {
            var model = mapper.Map<Platform>(platform);
            var response = await platformRepo.CreateFlatformAsync(model);
            var platformReadDto = mapper.Map<PlatformReadDto>(response);
            try
            {
                await CommandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Could not send Synchronously: {ex.Message}");
            }
            // Send message Async
            try {
                var publishDto = mapper.Map<PlatformPublishDto>(platformReadDto);
                publishDto.Event = "Platform_Published";
                MessageBusClient.PublishNewPlatform(publishDto);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Could not send Asynchronously: {ex.Message}");
            }
            return CreatedAtAction(nameof(GetPlatformsByIdAsync),new { id = response.Id }, response);
        }
    }
}
