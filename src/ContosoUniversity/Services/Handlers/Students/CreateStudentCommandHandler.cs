namespace ContosoUniversity.Services.Handlers.Students
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Students;

    using Domain.Contracts;
    using Domain.Student;

    using MediatR;

    public class CreateStudentCommandHandler : AsyncRequestHandler<CreateStudentCommand>
    {
        private readonly IStudentsRepository _repository;

        public CreateStudentCommandHandler(IStudentsRepository repository)
        {
            _repository = repository;
        }

        protected override Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            return _repository.Save(
                new Student(
                    request.LastName,
                    request.FirstName,
                    request.EnrollmentDate,
                    EnrollmentsCollection.Empty,
                    Guid.NewGuid()));
        }
    }
}