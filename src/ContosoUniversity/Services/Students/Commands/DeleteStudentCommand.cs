namespace ContosoUniversity.Services.Students.Commands;

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
        if (!await _studentsRepository.Exists(request.Id, cancellationToken))
            throw new EntityNotFoundException("student", request.Id);

        await _studentsRepository.Remove(request.Id, cancellationToken);
    }
}