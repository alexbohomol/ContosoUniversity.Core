namespace ContosoUniversity.Services.Instructors.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;

    using Domain.Contracts;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using ViewModels;
    using ViewModels.Instructors;

    public record GetInstructorEditFormQuery(Guid Id) : IRequest<EditInstructorForm>;
    
    public class GetInstructorEditFormQueryHandler : IRequestHandler<GetInstructorEditFormQuery, EditInstructorForm>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly ICoursesRepository _coursesRepository;

        public GetInstructorEditFormQueryHandler(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
        }
        
        public async Task<EditInstructorForm> Handle(GetInstructorEditFormQuery request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors
                                                      .Include(i => i.OfficeAssignment)
                                                      .Include(i => i.CourseAssignments)
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync(m => m.ExternalId == request.Id, cancellationToken);
            
            return instructor == null
                ? null
                : new EditInstructorForm
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