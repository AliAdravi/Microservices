using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext dbContext;
        public PlatformRepo(AppDbContext context)
        {
            dbContext = context;    
        }
        public async Task<Platform> CreateFlatformAsync(Platform flatform)
        {
            if(flatform == null)
                throw new ArgumentNullException(nameof(flatform));

            dbContext.Platforms.Add(flatform);
            await dbContext.SaveChangesAsync();
            return flatform;
        }

        public async Task<IEnumerable<Platform>> GetAllFlatformsAsync()
        {
            return await dbContext.Platforms.ToListAsync();
        }

        public async Task<Platform?> GetFlatformByIdAsync(int id)
        {
            return await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id );
        }

        public async Task<bool> SaveChanges()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
