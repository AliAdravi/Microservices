using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder appBuilder)
        {
            using var serviceScope = appBuilder.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient?.ReturnAllPlatforms();
            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>()!, platforms!);
        }

        private static async Task SeedData(ICommandRepo commandRepo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine(":: => Sending New Platforms");
            foreach(var platform in platforms)
            {
                if(!(await commandRepo.ExternalPlatformExistsAsync(platform.ExternalID)))
                {
                    await commandRepo.CreatePlatformAsync(platform);
                }
            }
        }
    }
}
