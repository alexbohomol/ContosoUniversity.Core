using IInstructorsRoRepository = Departments.Core.IInstructorsRoRepository;
using Instructor = Departments.Core.Projections.Instructor;

namespace ContosoUniversity.Application.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using SharedKernel.Exceptions;

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
