namespace Courses.Api.IntegrationTests;

using Xunit;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>;
