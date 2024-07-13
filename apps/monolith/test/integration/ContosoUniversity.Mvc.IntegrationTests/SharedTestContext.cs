namespace ContosoUniversity.Mvc.IntegrationTests;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;

using Xunit;

public class SharedTestContext : IAsyncLifetime
{
    private const string AppUrl = "http://localhost:10000";
    private const string DockerComposeRelativePath = "../../../../../../docker-compose.integration.yml";

    private static readonly string DockerComposePath =
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                (TemplateString)DockerComposeRelativePath));

    private readonly ICompositeService _dockerService = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposePath)
        .RemoveOrphans()
        .WaitForHttp("web-int-test", AppUrl)
        .Build();

    public HttpClient Client;

    public Task InitializeAsync()
    {
        _dockerService.Start();
        Client = new HttpClient { BaseAddress = new Uri(AppUrl) };

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _dockerService.Dispose();
        Client.Dispose();

        return Task.CompletedTask;
    }
}
