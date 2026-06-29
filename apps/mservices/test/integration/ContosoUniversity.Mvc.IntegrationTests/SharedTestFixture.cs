namespace ContosoUniversity.Mvc.IntegrationTests;

using Xunit;

[CollectionDefinition(nameof(SharedTestFixture))]
public class SharedTestFixture : ICollectionFixture<SharedTestContext>;
