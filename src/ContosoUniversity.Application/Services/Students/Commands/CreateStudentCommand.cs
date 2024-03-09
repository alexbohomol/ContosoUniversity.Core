namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadWrite;

using Domain.Student;

using MediatR;

public class CreateStudentCommand : IRequest
{
    public DateTime EnrollmentDate { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}

internal class CreateStudentCommandHandler(IStudentsRwRepository repository) : IRequestHandler<CreateStudentCommand>
{
    public async Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        await repository.Save(Student.Create(
                request.LastName,
                request.FirstName,
                request.EnrollmentDate),
            cancellationToken);
    }
}
