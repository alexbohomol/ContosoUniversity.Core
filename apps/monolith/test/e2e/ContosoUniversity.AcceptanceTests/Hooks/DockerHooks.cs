namespace ContosoUniversity.AcceptanceTests.Hooks;

using System.Threading.Tasks;

using FluentDocker.Kernel;
using FluentDocker.Testing.Core;
using FluentDocker.Testing.NUnit;

using Microsoft.Extensions.Configuration;

using Reqnroll;

[Binding]
public class DockerHooks
{
    private static FluentDockerKernel _kernel;
    private static ComposeResource _resource;

    [BeforeFeature]
    public static async Task StartDockerInfrastructure(IConfiguration configuration)
    {
        (_kernel, _resource) = await NUnitResourceHelpers.CreateComposeAsync(builder => builder
            .WithComposeFiles([
                "../../../../../../docker-compose.yml",
                "../../../../../../docker-compose.override.yml"
            ])
            .WithRemoveOrphans()
            .WithForceRecreate()
            .WithWait());
    }

    [AfterFeature]
    public static async Task DisposeDockerInfrastructure()
    {
        await NUnitResourceHelpers.DisposeAsync(_resource, _kernel);
    }
}
