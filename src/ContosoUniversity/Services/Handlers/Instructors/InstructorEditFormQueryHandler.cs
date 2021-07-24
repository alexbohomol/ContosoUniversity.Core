namespace ContosoUniversity.Services.Handlers.Instructors
{
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Instructors;

    using ViewModels;
    using ViewModels.Instructors;

    public class InstructorEditFormQueryHandler : IRequestHandler<InstructorEditFormQuery, InstructorEditForm>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly ICoursesRepository _coursesRepository;

        public InstructorEditFormQueryHandler(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
        }
        
        public async Task<InstructorEditForm> Handle(InstructorEditFormQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == request.Id, cancellationToken);
            
            return instructor == null
                ? null
                : new InstructorEditForm
                {
                    ExternalId = instructor.ExternalId,
                    LastName = instructor.LastName,
                    FirstName = instructor.FirstMidName,
                    HireDate = instructor.HireDate,
                    Location = instructor.OfficeAssignment?.Location,
                    AssignedCourses = (await _coursesRepository.GetAll()).ToAssignedCourseOptions(instructor)
                };
        }
    }
}