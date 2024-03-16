namespace ContosoUniversity.Application.Services.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetInstructorDetailsQuery(Guid Id) : IRequest<Instructor>;

internal class GetInstructorDetailsQueryHandler(IInstructorsRoRepository instructorsRepository)
    : IRequestHandler<GetInstructorDetailsQuery, Instructor>
{
    public async Task<Instructor> Handle(
        GetInstructorDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Instructor instructor = await instructorsRepository.GetById(request.Id, cancellationToken);

        return instructor ?? throw new EntityNotFoundException(nameof(instructor), request.Id);
    }
}
