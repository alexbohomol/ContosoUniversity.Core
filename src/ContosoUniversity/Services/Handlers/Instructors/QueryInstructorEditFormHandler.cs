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

    public class QueryInstructorEditFormHandler : IRequestHandler<QueryInstructorEditForm, InstructorEditForm>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly ICoursesRepository _coursesRepository;

        public QueryInstructorEditFormHandler(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
        }
        
        public async Task<InstructorEditForm> Handle(QueryInstructorEditForm request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExternalId == request.Id, cancellationToken);
            if (instructor == null)
            {
                return null;
            }

            return new InstructorEditForm
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