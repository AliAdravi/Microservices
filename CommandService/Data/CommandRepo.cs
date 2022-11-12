using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        public AppDbContext DbContext { get; }
        public CommandRepo(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task CreateCommandAsync(int platformId, Command command)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            DbContext.Commands.Add(command);
            await DbContext.SaveChangesAsync();
        }

        public async Task CreatePlatformAsync(Platform platform)
        {
            if(platform == null)
                throw new ArgumentNullException(nameof(platform));

            DbContext.Platforms.Add(platform);
            await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
        {
            return await DbContext.Platforms.ToListAsync();
        }

        public async Task<Command?> GetCommandAsync(int platformId, int commandId)
        {
            return await DbContext.Commands
                .FirstOrDefaultAsync(c => c.PlatformId == platformId 
                    && c.Id == commandId);
        }

        public async Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId)
        {
            return await DbContext.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy( c => c.Platform.Name)
                .ToListAsync();
        }

        public async Task<bool> PlatformExistsAsync(int platformId)
        {
            return await DbContext.Platforms
                .AnyAsync(p => p.Id == platformId);
        }
        public async Task<bool> ExternalPlatformExistsAsync(int externalPlatformId)
        {
            return await DbContext.Platforms
                .AnyAsync(p => p.ExternalID == externalPlatformId);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await DbContext.SaveChangesAsync()) >= 0 ;
        }
    }
}
