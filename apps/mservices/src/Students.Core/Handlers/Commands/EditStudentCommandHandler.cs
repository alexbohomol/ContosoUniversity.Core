namespace Students.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

using MediatR;

internal class EditStudentCommandHandler(
    IStudentsRwRepository studentsRepository)
    : IRequestHandler<EditStudentCommand, Student>
{
    public async Task<Student> Handle(
        EditStudentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Student student = await studentsRepository.GetById(request.ExternalId, cancellationToken);

        ArgumentNullException.ThrowIfNull(student);

        student.UpdatePersonInfo(request.LastName, request.FirstName);
        student.Enroll(request.EnrollmentDate);

        await studentsRepository.Save(student, cancellationToken);

        return student;
    }
}
