namespace ContosoUniversity.Services.Instructors.Handlers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Commands;

    using Data.Departments;
    using Data.Departments.Models;

    using MediatR;

    public class CreateInstructorCommandHandler : AsyncRequestHandler<CreateInstructorCommand>
    {
        private readonly DepartmentsContext _departmentsContext;

        public CreateInstructorCommandHandler(DepartmentsContext departmentsContext)
        {
            _departmentsContext = departmentsContext;
        }
        
        protected override async Task Handle(CreateInstructorCommand command, CancellationToken cancellationToken)
        {
            var instructor = new Instructor
            {
                ExternalId = Guid.NewGuid(),
                FirstMidName = command.FirstName,
                LastName = command.LastName,
                HireDate = command.HireDate,
                OfficeAssignment = command.HasAssignedOffice
                    ? new OfficeAssignment {Location = command.Location}
                    : null
            };

            instructor.CourseAssignments = command.SelectedCourses?.Select(x => new CourseAssignment
            {
                InstructorId = instructor.Id, // not yet generated ???
                CourseExternalId = Guid.Parse((string)x)
            }).ToList();

            _departmentsContext.Add(instructor);
            
            await _departmentsContext.SaveChangesAsync();
        }
    }
}