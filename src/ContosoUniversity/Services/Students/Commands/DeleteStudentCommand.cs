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
    private readonly IStudentsRoRepository _studentsRoRepository;
    private readonly IStudentsRwRepository _studentsRwRepository;

    public DeleteStudentCommandHandler(
        IStudentsRoRepository studentsRoRepository,
        IStudentsRwRepository studentsRwRepository)
    {
        _studentsRoRepository = studentsRoRepository;
        _studentsRwRepository = studentsRwRepository;
    }

    protected override async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        if (!await _studentsRoRepository.Exists(request.Id, cancellationToken))
            throw new EntityNotFoundException("student", request.Id);

        await _studentsRwRepository.Remove(request.Id, cancellationToken);
    }
}