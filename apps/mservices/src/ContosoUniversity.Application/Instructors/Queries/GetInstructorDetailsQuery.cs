namespace ContosoUniversity.Application.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

using SharedKernel.Exceptions;

public record GetInstructorDetailsQuery(Guid Id) : IRequest<Instructor>;

internal class GetInstructorDetailsQueryHandler(IInstructorsApiClient client)
    : IRequestHandler<GetInstructorDetailsQuery, Instructor>
{
    public async Task<Instructor> Handle(
        GetInstructorDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Instructor instructor = await client.GetById(request.Id, cancellationToken);

        return instructor ?? throw new EntityNotFoundException(nameof(instructor), request.Id);
    }
}
