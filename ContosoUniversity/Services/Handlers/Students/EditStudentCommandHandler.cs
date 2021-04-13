namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Students;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    public class EditStudentCommandHandler : AsyncRequestHandler<EditStudentCommand>
    {
        private readonly IStudentsRepository _studentsRepository;

        public EditStudentCommandHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        protected override async Task Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.ExternalId);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.ExternalId);

            student.Update(
                request.LastName,
                request.FirstName,
                request.EnrollmentDate);

            await _studentsRepository.Save(student);
        }
    }
}