namespace ContosoUniversity.Mvc.IntegrationTests;

using Xunit;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>;
