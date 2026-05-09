namespace IntegrationTesting.SharedKernel;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqClient(string connectionString)
{
    private readonly ConnectionFactory _factory = new() { Uri = new Uri(connectionString) };

    private static readonly JsonSerializerOptions CamelCaseOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static readonly JsonSerializerOptions PropertyCaseInsensitiveOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task PublishAsync<TMessage>(string queueName, TMessage message) where TMessage : class
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var envelope = new MessageEnvelope<TMessage> { Message = message };
        var bodyJson = JsonSerializer.Serialize(envelope, CamelCaseOptions);
        var body = Encoding.UTF8.GetBytes(bodyJson);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);
    }

    public async Task<TMessage> TryConsumeAsync<TMessage>(string queueName) where TMessage : class
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var tcs = new TaskCompletionSource<TMessage>();

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var bodyJson = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received raw message: {bodyJson}");

                var envelope = JsonSerializer.Deserialize<MessageEnvelope<TMessage>>(bodyJson, PropertyCaseInsensitiveOptions);
                if (envelope?.Message is { } message)
                {
                    tcs.TrySetResult(message);
                }
                else
                {
                    tcs.TrySetException(new InvalidOperationException("Failed to extract the message from the envelope."));
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            await Task.Yield();
        };

        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

        return await tcs.Task;
    }
}

file class MessageEnvelope<TMessage>
{
    [JsonPropertyName("message")]
    public required TMessage Message { get; init; }
}
