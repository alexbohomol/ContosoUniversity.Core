using IStudentsRoRepository = Students.Core.IStudentsRoRepository;
using Student = Students.Core.Projections.Student;

namespace ContosoUniversity.Application.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SharedKernel.Exceptions;

public record GetStudentProjectionQuery(Guid Id) : IRequest<Student>;

internal class GetStudentEditFormQueryHandler(IStudentsRoRepository studentsRepository)
    : IRequestHandler<GetStudentProjectionQuery, Student>
{
    public async Task<Student> Handle(
        GetStudentProjectionQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Student student = await studentsRepository.GetById(request.Id, cancellationToken);

        return student ?? throw new EntityNotFoundException(nameof(student), request.Id);
    }
}
