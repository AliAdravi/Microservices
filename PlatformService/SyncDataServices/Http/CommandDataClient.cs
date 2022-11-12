using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{configuration["CommandService"]}/api/v1/Platforms/ping", httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(":: => Sync Post to command was Ok!");
            }
            else
            {
                Console.WriteLine(":: => Sync Post to command was Failed!");
            }
        }
    }
}