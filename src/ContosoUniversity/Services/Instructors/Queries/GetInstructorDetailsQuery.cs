namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Instructors;

    public record GetInstructorDetailsQuery(Guid Id) : IRequest<InstructorDetailsViewModel>;
    
    public class GetInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, InstructorDetailsViewModel>
    {
        private readonly DepartmentsContext _departmentsContext;

        public GetInstructorDetailsQueryHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        public async Task<InstructorDetailsViewModel> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors.FirstOrDefaultAsync(m => m.ExternalId == request.Id, cancellationToken);
            if (instructor == null)
                throw new EntityNotFoundException(nameof(instructor), request.Id);

            return new InstructorDetailsViewModel
            {
                LastName = instructor.LastName,
                FirstName = instructor.FirstMidName,
                HireDate = instructor.HireDate,
                ExternalId = instructor.ExternalId
            };
        }
    }
}