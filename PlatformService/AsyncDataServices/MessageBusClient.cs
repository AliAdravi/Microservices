using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient: IMessageBusClient
    {
        private IConfiguration Configuration { get; }
        private IConnection Connection { get; } = default!;
        private IModel Channel { get; } = default!;
        public MessageBusClient(IConfiguration configuration)
        {
            Configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:Host"],
                Port = int.Parse(configuration["RabbitMQ:Port"])
            };

            try
            {
                Connection = factory.CreateConnection();
                Channel = Connection.CreateModel();
                Channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                Connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine(":: => Connected to Message Bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine(":: => Could not connect to message bus,", ex.Message);
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine(":: => RabbitMQ Connection Shutdown!");
        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            if (Connection.IsOpen)
            {
                Console.WriteLine(":: RBMQ Connectoin is open, send the messages");
                SendMessage(message);

            } else
            {
                Console.WriteLine(":: => RBMQ Connectoin is Close, (:)(:)");
            }
        }
        public void Dispose()
        {
            Console.WriteLine(":: => Disposing....");
            Channel?.Close();
            Connection?.Close();
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Channel.BasicPublish(exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);
            Console.WriteLine(":: => Message sent");
        }
    }
}
