using Student = Students.Core.Domain.Student;

namespace Students.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class CreateStudentCommandHandler(
    IStudentsRwRepository repository)
    : IRequestHandler<CreateStudentCommand>
{
    public async Task Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await repository.Save(Student.Create(
                request.LastName,
                request.FirstName,
                request.EnrollmentDate),
            cancellationToken);
    }
}
