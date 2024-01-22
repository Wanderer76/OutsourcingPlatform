using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace OutsourcePlatformApp.Service.MessageBrocker;

public interface IMessageProducer
{
    void SendMessage<T>(T message, string type);
}

public class MessageSender : IMessageProducer
{
    public void SendMessage<T>(T message, string type)
    {
        var isRunningInContainer =
            bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) &&
            inDocker;
        var factory = new ConnectionFactory
        {
            HostName = isRunningInContainer ? "rabbitmq" : "localhost",
            Port = 5672,
            Password = "guest",
            UserName = "guest",
            VirtualHost = "/"
        };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare("NotificationQueue");

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        channel.BasicPublish(exchange: "notification-exchange", routingKey: type, body: body);
    }
}