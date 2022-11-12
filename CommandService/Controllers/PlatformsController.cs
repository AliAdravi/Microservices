using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;
using static CommandService.Dtos.PlatformDto;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlatformsController : Controller
    {
        private ICommandRepo CommandRepo { get; }

        private IMapper Mapper { get; }

        public PlatformsController(ICommandRepo commandRepo, IMapper mapper)
        {
            CommandRepo = commandRepo;
            Mapper = mapper;
        }
        [HttpGet("GetPlatforms")]
        public async Task<IActionResult> GetPlatforms()
        {
            Console.WriteLine(":: => Getting platforms from Command Service");
            var platforms = await CommandRepo.GetAllPlatformsAsync();
            return Ok(Mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }


        [HttpPost("ping")]
        public IActionResult Ping()
        {
            Console.WriteLine("=====>>> Inbound post # Command Service");
            return Ok(":: => Platforms Controller Ok");
        }
    }
}
