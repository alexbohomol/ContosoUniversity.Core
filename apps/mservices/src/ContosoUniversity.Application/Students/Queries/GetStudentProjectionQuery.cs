namespace ContosoUniversity.Application.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using ApiClients;

using MediatR;

using SharedKernel.Exceptions;

public record GetStudentProjectionQuery(Guid Id) : IRequest<Student>;

internal class GetStudentEditFormQueryHandler(IStudentsApiClient client)
    : IRequestHandler<GetStudentProjectionQuery, Student>
{
    public async Task<Student> Handle(
        GetStudentProjectionQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Student student = await client.GetById(request.Id, cancellationToken);

        return student ?? throw new EntityNotFoundException(nameof(student), request.Id);
    }
}
