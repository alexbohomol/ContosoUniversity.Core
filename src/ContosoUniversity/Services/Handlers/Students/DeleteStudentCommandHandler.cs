namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Threading;
    using System.Threading.Tasks;

    using Commands.Students;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    public class DeleteStudentCommandHandler : AsyncRequestHandler<DeleteStudentCommand>
    {
        private readonly IStudentsRepository _studentsRepository;

        public DeleteStudentCommandHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        protected override async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            await _studentsRepository.Remove(student.EntityId);
        }
    }
}