namespace ContosoUniversity.Application.Services.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetInstructorDetailsQuery(Guid Id) : IRequest<Instructor>;

public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, Instructor>
{
    private readonly IInstructorsRoRepository _instructorsRepository;

    public GetInstructorDetailsQueryHandler(IInstructorsRoRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task<Instructor> Handle(
        GetInstructorDetailsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Instructor instructor = await _instructorsRepository.GetById(request.Id, cancellationToken);

        return instructor ?? throw new EntityNotFoundException(nameof(instructor), request.Id);
    }
}
