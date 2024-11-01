namespace ContosoUniversity.Application.Services.Students.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using global::Students.Core;

using MediatR;

public record DeleteStudentCommand(Guid Id) : IRequest;

internal class DeleteStudentCommandHandler(
    IStudentsRwRepository studentsRwRepository)
    : IRequestHandler<DeleteStudentCommand>
{
    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        await studentsRwRepository.Remove(request.Id, cancellationToken);
    }
}
