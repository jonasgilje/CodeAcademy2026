using CodeAcademy.DotnetConsumer.Common.Config;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

Console.WriteLine("Producer starting...");
// Establish connection to RabbitMQ
using var connection = await ConnectionHelper.ConnectAsync();
Console.WriteLine("Connected to RabbitMQ");

// Implement a basic producer here.
// Start with:
// - Create a channel
// - Declare a queue
// - Publish a message to the queue (you can use a simple JSON string as the message body)


// FANOUT
// 1. Utvid `Producer` til å opprette en `Fanout` exchange. Sørg for at meldinger publiseres til denne exchangen med et gitt intervall, feks 2 sekunder. `channel.ExchangeDeclareAsync`, `channel.QueueDeclareAsync` og `channel.BasicPublishAsync` kan komme hendig med her.

// Create a channel and declare the queue
using var channel = await connection.CreateChannelAsync();
await channel.ExchangeDeclareAsync(exchange: "idem-exchange-fanout", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
await channel.QueueDeclareAsync(queue: "idem-events", durable: true, exclusive: false, autoDelete: false, arguments: null);

// Publish messages to the queue with for loop to simulate multiple events
for (int i = 0; i < 10; i++)
{
    var message = $"Idems Event {i + 1} at {DateTime.Now}";

    var messageBody = JsonSerializer.Serialize(message);
    var body = Encoding.UTF8.GetBytes(messageBody);

    await channel.BasicPublishAsync(exchange: "idem-exchange-fanout", routingKey: string.Empty, mandatory: true, basicProperties: new BasicProperties { Persistent = true }, body: body);
    Console.WriteLine($"Published event: {message}");

    await Task.Delay(2000);
}

Console.WriteLine("Producer finished.");