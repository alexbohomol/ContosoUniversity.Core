namespace IntegrationTesting.SharedKernel;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqClient(string connectionString)
{
    private readonly ConnectionFactory _factory = new() { Uri = new Uri(connectionString) };

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

                var envelope = JsonSerializer.Deserialize<MessageEnvelope<TMessage>>(bodyJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

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

    internal class MessageEnvelope<TMessage>
    {
        [JsonPropertyName("message")]
        public TMessage Message { get; set; }
    }
}
