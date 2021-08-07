namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;
    using System.Linq;
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
            var instructor = await _instructorsRepository.GetById(request.Id);
            
            return instructor == null
                ? null
                : new EditInstructorForm
                {
                    ExternalId = instructor.EntityId,
                    LastName = instructor.LastName,
                    FirstName = instructor.FirstName,
                    HireDate = instructor.HireDate,
                    Location = instructor.Office?.Title,
                    AssignedCourses = (await _coursesRepository.GetAll()).ToAssignedCourseOptions(instructor.Courses.Select(x => x.CourseId))
                };
        }
    }
}