using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos
{
    public class CommandDtos
    {
        public record CommandReadDto(
            int Id,
            string HowTo,
            string CommandLine,
            int PlatformId);

        public record CommandCreateDto(
            [Required] string HowTo,
            [Required] string CommandLine);
    }
}
