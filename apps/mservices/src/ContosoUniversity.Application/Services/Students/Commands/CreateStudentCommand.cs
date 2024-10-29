namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Student;

using MediatR;

public record CreateStudentCommand(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName) : IRequest;

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
