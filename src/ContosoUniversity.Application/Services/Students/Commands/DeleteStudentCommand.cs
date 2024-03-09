namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadWrite;

using Exceptions;

using MediatR;

public record DeleteStudentCommand(Guid Id) : IRequest;

internal class DeleteStudentCommandHandler(
    IStudentsRoRepository studentsRoRepository,
    IStudentsRwRepository studentsRwRepository) : IRequestHandler<DeleteStudentCommand>
{
    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        if (!await studentsRoRepository.Exists(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException("student", request.Id);
        }

        await studentsRwRepository.Remove(request.Id, cancellationToken);
    }
}
