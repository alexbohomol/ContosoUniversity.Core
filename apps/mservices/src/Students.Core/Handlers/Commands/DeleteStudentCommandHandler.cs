namespace Students.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.Messaging.Contracts.Commands;

using MediatR;

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
