namespace ContosoUniversity.AcceptanceTests.Hooks;

using System.IO;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;

using Microsoft.Extensions.Configuration;

using TechTalk.SpecFlow;

[Binding]
public class DockerHooks
{
    private static readonly string DockerComposePath =
        Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                (TemplateString)"../../../../../../docker-compose.integration.yml"));

    private static ICompositeService _dockerService;

    [BeforeFeature]
    public static void StartDockerInfrastructure(IConfiguration configuration)
    {
        _dockerService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(DockerComposePath)
            .RemoveOrphans()
            .WaitForHttp("web-int-test", configuration["PageBaseUrl:Http"])
            .Build();

        _dockerService.Start();
    }

    [AfterFeature]
    public static void DisposeDockerInfrastructure()
    {
        _dockerService.Dispose();
    }
}
