using System.ComponentModel.DataAnnotations;

namespace CommandService.Models
{
    public record Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public ICollection<Command> Commands { get; set; } = default!;
    }

    public record Command {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string HowTo { get; set; } = default!;

        [Required]
        public string CommandLine { get; set; } = default!;

        [Required]
        public int PlatformId { get; set; }

        public Platform Platform { get; set; } = default!;
    }
}
