using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        Task<bool> SaveChanges();
        Task<IEnumerable<Platform>> GetAllFlatformsAsync();
        Task<Platform?> GetFlatformByIdAsync(int id);

        Task<Platform> CreateFlatformAsync(Platform flatform);

    }
}
