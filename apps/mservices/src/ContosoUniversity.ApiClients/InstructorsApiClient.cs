namespace ContosoUniversity.ApiClients;

using Application.ApiClients;

internal class InstructorsApiClient(HttpClient client) : IInstructorsApiClient
{
    public Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Instructor[]> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Instructor> GetById(Guid externalId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
