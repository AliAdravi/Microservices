using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos
{
    public record PlatformReadDto(
        int Id,
        string Name,
        string Publisher,
        string Cost);

    public record PlatformCreateDto(
        [Required]
        string Name,
        [Required]
        string Publisher,
        [Required]
        string Cost);
}
