using System.Diagnostics;
using System.Text;
using System.Text.Json;
using NotificationApp.Models;
using NotificationApp.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationApp.BackgroundService;

public class BrokerListener : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly NotificationService notificationService;
    private readonly IConnection connection;
    private readonly IModel channel;

    public BrokerListener(IServiceProvider service)
    {
        notificationService = service.CreateScope().ServiceProvider.GetRequiredService<NotificationService>();
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
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        channel.QueueDeclare(
            queue: "NotificationQueue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            // Каким-то образом обрабатываем полученное сообщение
            var actionNotification = JsonSerializer
                .Deserialize<ActionNotificationModel>(content);
            if (ea.RoutingKey.Equals("chat"))
            {
                await notificationService.CreateChatNotification(JsonSerializer
                    .Deserialize<ChatNotificationModel>(content));
            }
            else
            {
                await notificationService.CreateActionNotification(JsonSerializer
                    .Deserialize<ActionNotificationModel>(content));
            }

            channel.BasicAck(ea.DeliveryTag, false);
        };
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        channel.Close();
        connection.Close();
        base.Dispose();
    }
}