namespace ContosoUniversity.Mvc.IntegrationTests;

using Xunit;

/// <summary>
/// Serializes SQL-backed test classes while preserving their separate class fixtures.
/// </summary>
[CollectionDefinition(nameof(SqlServerTestGroup))]
public class SqlServerTestGroup
{
}
