using AutoMapper;
using CommandService.Data;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using static CommandService.Dtos.CommandDtos;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/v1/platforms/{platformId}/[controller]")]
    public class CommandsController : Controller
    {
        public ICommandRepo CommandRepo { get; }
        public IMapper Mapper { get; }
        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            CommandRepo = commandRepo;
            Mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> Get(int platformId)
        {
            Console.WriteLine($":: => Command-Get {platformId}");
            if (platformId == 0)
                return BadRequest();
            if(!(await CommandRepo.PlatformExistsAsync(platformId)))
                return NoContent();

            var commands = await CommandRepo.GetCommandsForPlatformAsync(platformId);
            return Ok(Mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult<CommandReadDto>>  GetGetCommandForPlatform
            (int platformId, int commandId)
        {
            Console.WriteLine($":: => Command-Get by PID: {platformId} & CID: {commandId}");
            if (platformId == 0 || commandId == 0)
                return BadRequest();
            if (!(await CommandRepo.PlatformExistsAsync(platformId)))
                return NoContent();

            var command = await CommandRepo.GetCommandAsync(platformId, commandId);
            return Ok(Mapper.Map<CommandReadDto>(command));
        }

        [HttpPost("CreateCommandForPlatform")]
        public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform(int platformId, CommandCreateDto command)
        {
            Console.WriteLine($":: => CreateCommandForPlatform {platformId}");
            if (platformId == 0 || command == null)
                return BadRequest();
            if (!(await CommandRepo.PlatformExistsAsync(platformId)))
                return NotFound($"Platform {platformId} not found!");
            var cmd = Mapper.Map<Command>(command);
            await CommandRepo.CreateCommandAsync(platformId, cmd);

            var readDto = Mapper.Map<CommandReadDto>(cmd);
            return CreatedAtAction("GetGetCommandForPlatform",
                new { platformId = platformId, commandId = readDto.Id }, readDto);
        }

    }
}
