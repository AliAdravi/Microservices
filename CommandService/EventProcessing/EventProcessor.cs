using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFatory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFatory = scopeFactory;
            _mapper = mapper;
        }
        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    await AddPlatform(message);
                    break;
                case EventType.Udetermined:
                    break;
            }
        }

        private async Task AddPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFatory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDto = 
                JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);
            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);
                if(!await repo.ExternalPlatformExistsAsync(platform.ExternalID))
                {
                   await repo.CreatePlatformAsync(platform);
                } else
                {
                    Console.WriteLine(":: => External Platform Already Exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($":: => Could not add platform to DB. \n{ex.Message}");
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType?.Event)
            {
                case "Platform_Published":
                    return EventType.PlatformPublished;
                default:
                    return EventType.Udetermined;

            }
        }
        enum EventType
        {
            PlatformPublished,
            Udetermined
        }
    }
}
