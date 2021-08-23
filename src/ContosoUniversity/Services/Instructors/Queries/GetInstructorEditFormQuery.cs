namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;

    using MediatR;

    using ViewModels;
    using ViewModels.Instructors;

    public record GetInstructorEditFormQuery(Guid Id) : IRequest<EditInstructorForm>;
    
    public class GetInstructorEditFormQueryHandler : IRequestHandler<GetInstructorEditFormQuery, EditInstructorForm>
    {
        private readonly IInstructorsRepository _instructorsRepository;
        private readonly ICoursesRepository _coursesRepository;

        public GetInstructorEditFormQueryHandler(
            IInstructorsRepository instructorsRepository,
            ICoursesRepository coursesRepository)
        {
            _instructorsRepository = instructorsRepository;
            _coursesRepository = coursesRepository;
        }
        
        public async Task<EditInstructorForm> Handle(GetInstructorEditFormQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _instructorsRepository.GetById(request.Id, cancellationToken);
            if (instructor is null)
                return null;

            var courses = await _coursesRepository.GetAll(cancellationToken);
            
            return new EditInstructorForm
            {
                ExternalId = instructor.ExternalId,
                LastName = instructor.LastName,
                FirstName = instructor.FirstName,
                HireDate = instructor.HireDate,
                Location = instructor.Office?.Title,
                AssignedCourses = courses.ToAssignedCourseOptions(instructor.Courses)
            };
        }
    }
}