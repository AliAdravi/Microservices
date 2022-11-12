using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider?.GetService<AppDbContext>()!, isProd);
        }

        private static void SeedData(AppDbContext dbContext, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine(":: => Attempting to apply migration");
                try
                {
                    dbContext.Database.Migrate();
                }catch (Exception ex)
                {
                    Console.WriteLine($":: =>> Could not migrate error: {ex.Message}");
                }
            }
            if (!dbContext.Platforms.Any())
            {
                Console.WriteLine("=> Seeding data...");
                dbContext.Platforms.AddRange(
                    new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("=> We already have data inserted");
            }
        }
    }
}
