namespace Courses.Api.IntegrationTests;

using DotNet.Testcontainers.Builders;

using Testcontainers.RabbitMq;

public class RabbitMqContext : IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:management")
        .WithUsername("guest")
        .WithPassword("guest")
        .WithPortBinding(5672, 5672)
        .WithPortBinding(15672, 15672)
        .WithWaitStrategy(
            Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(5672)
                .UntilCommandIsCompleted("rabbitmq-diagnostics check_port_connectivity"))
        .Build();

    public async Task InitializeAsync()
    {
        await _rabbitMqContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _rabbitMqContainer.DisposeAsync();
    }
}
