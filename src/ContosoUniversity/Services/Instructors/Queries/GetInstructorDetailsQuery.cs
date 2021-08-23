namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using ViewModels.Instructors;

    public record GetInstructorDetailsQuery(Guid Id) : IRequest<InstructorDetailsViewModel>;
    
    public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetailsViewModel>
    {
        private readonly IInstructorsRepository _instructorsRepository;

        public GetInstructorDetailsQueryHandler(IInstructorsRepository instructorsRepository)
        {
            _instructorsRepository = instructorsRepository;
        }
        
        public async Task<InstructorDetailsViewModel> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _instructorsRepository.GetById(request.Id, cancellationToken);
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
}