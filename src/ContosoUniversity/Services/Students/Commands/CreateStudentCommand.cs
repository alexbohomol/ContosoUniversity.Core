namespace ContosoUniversity.Services.Students.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Student;

using MediatR;

public class CreateStudentCommand : IRequest
{
    public DateTime EnrollmentDate { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}

public class CreateStudentCommandHandler : AsyncRequestHandler<CreateStudentCommand>
{
    private readonly IStudentsRwRepository _repository;

    public CreateStudentCommandHandler(IStudentsRwRepository repository)
    {
        _repository = repository;
    }

    protected override Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        return _repository.Save(Student.Create(
                request.LastName,
                request.FirstName,
                request.EnrollmentDate),
            cancellationToken);
    }
}