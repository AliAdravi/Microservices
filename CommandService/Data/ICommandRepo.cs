using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        Task<bool> SaveChangesAsync();

        // Platform Related
        Task<IEnumerable<Platform>> GetAllPlatformsAsync(); 
        Task CreatePlatformAsync(Platform platform);
        Task<bool> PlatformExistsAsync(int platformId);
        Task<bool> ExternalPlatformExistsAsync(int externalPlatformId);

        // Command related
        Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId);
        Task<Command?> GetCommandAsync(int platformId, int commandId);
        Task CreateCommandAsync(int platformId, Command command);
    }
}
