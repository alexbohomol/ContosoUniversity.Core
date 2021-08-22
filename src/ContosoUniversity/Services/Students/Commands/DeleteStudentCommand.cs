namespace ContosoUniversity.Services.Students.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    public record DeleteStudentCommand(Guid Id) : IRequest;
    
    public class DeleteStudentCommandHandler : AsyncRequestHandler<DeleteStudentCommand>
    {
        private readonly IStudentsRepository _studentsRepository;

        public DeleteStudentCommandHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        protected override async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id, cancellationToken);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            await _studentsRepository.Remove(student.EntityId, cancellationToken);
        }
    }
}