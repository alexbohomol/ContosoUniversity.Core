namespace ContosoUniversity.SystemTests;

using System.Threading.Tasks;

using FluentDocker.Kernel;
using FluentDocker.Testing.Core;
using FluentDocker.Testing.NUnit;

using NUnit.Framework;

[SetUpFixture]
public class DockerSetUpFixture
{
    private FluentDockerKernel _kernel;
    private ComposeResource _resource;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
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

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await NUnitResourceHelpers.DisposeAsync(_resource, _kernel);
    }
}
