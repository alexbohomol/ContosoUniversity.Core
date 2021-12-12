namespace ContosoUniversity.Services.Instructors.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Instructor;

using MediatR;

using ViewModels.Instructors;

public record GetInstructorDetailsQuery(Guid Id) : IRequest<InstructorDetailsViewModel>;

public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetailsViewModel>
{
    private readonly IInstructorsRoRepository _instructorsRepository;

    public GetInstructorDetailsQueryHandler(IInstructorsRoRepository instructorsRepository)
    {
        _instructorsRepository = instructorsRepository;
    }

    public async Task<InstructorDetailsViewModel> Handle(GetInstructorDetailsQuery request,
        CancellationToken cancellationToken)
    {
        Instructor instructor = await _instructorsRepository.GetById(request.Id, cancellationToken);
        if (instructor == null)
            throw new EntityNotFoundException(nameof(instructor), request.Id);

        return new InstructorDetailsViewModel
        {
            LastName = instructor.LastName,
            FirstName = instructor.FirstName,
            HireDate = instructor.HireDate,
            ExternalId = instructor.ExternalId
        };
    }
}