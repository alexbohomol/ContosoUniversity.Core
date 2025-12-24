namespace Departments.Api.IntegrationTests;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

using IntegrationTesting.SharedKernel;

using Testcontainers.RabbitMq;

public class RabbitMqContext : IAsyncLifetime
{
    private const string RabbitmqConf = "../../../../../../../../rabbitmq.conf";
    private const string DefinitionsJson = "../../../../../../../../definitions.json";

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:management")
        .WithUsername("guest")
        .WithPassword("guest")
        .WithBindMount(RabbitmqConf.ToAbsolutePath(), "/etc/rabbitmq/rabbitmq.conf", AccessMode.ReadOnly)
        .WithBindMount(DefinitionsJson.ToAbsolutePath(), "/etc/rabbitmq/definitions.json", AccessMode.ReadOnly)
        // .WithPortBinding(5672, 5672)
        // .WithPortBinding(15672, 15672)
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

    public string ConnectionString => _rabbitMqContainer.GetConnectionString();
}
