namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using Exceptions;

using MediatR;

public record DeleteStudentCommand(Guid Id) : IRequest;

internal class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IStudentsRoRepository _studentsRoRepository;
    private readonly IStudentsRwRepository _studentsRwRepository;

    public DeleteStudentCommandHandler(
        IStudentsRoRepository studentsRoRepository,
        IStudentsRwRepository studentsRwRepository)
    {
        _studentsRoRepository = studentsRoRepository;
        _studentsRwRepository = studentsRwRepository;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        if (!await _studentsRoRepository.Exists(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("student", request.Id);
        }

        await _studentsRwRepository.Remove(request.Id, cancellationToken);
    }
}
