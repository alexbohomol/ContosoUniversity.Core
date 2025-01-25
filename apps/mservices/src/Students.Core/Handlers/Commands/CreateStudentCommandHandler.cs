namespace Students.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class CreateStudentCommandHandler(
    IStudentsRwRepository repository)
    : IRequestHandler<CreateStudentCommand, Student>
{
    public async Task<Student> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var student = Student.Create(
            request.LastName,
            request.FirstName,
            request.EnrollmentDate);

        await repository.Save(student, cancellationToken);

        return student;
    }
}
