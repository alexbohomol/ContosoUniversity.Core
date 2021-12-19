namespace ContosoUniversity.Services.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Exceptions;

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
        if (instructor == null)
            throw new EntityNotFoundException(nameof(instructor), request.Id);

        return instructor;
    }
}