namespace ContosoUniversity.Services.Instructors.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Data.Departments;
    using Data.Departments.Models;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    public class EditInstructorCommand : IRequest
    {
        public Guid ExternalId { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "The first character must upper case and the remaining characters must be alphabetical")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public string[] SelectedCourses { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public bool HasAssignedOffice => !string.IsNullOrWhiteSpace(Location);
    }
    
        public class EditInstructorCommandHandler : AsyncRequestHandler<EditInstructorCommand>
    {
        private readonly DepartmentsContext _departmentsContext;
        private readonly ICoursesRepository _coursesRepository;

        public EditInstructorCommandHandler(
            DepartmentsContext departmentsContext,
            ICoursesRepository coursesRepository)
        {
            _departmentsContext = departmentsContext;
            _coursesRepository = coursesRepository;
        }
        
        protected override async Task Handle(EditInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructor = await _departmentsContext.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .FirstOrDefaultAsync(m => m.ExternalId == request.ExternalId, cancellationToken);
            if (instructor is null)
                throw new EntityNotFoundException(nameof(instructor), request.ExternalId);

            instructor.FirstMidName = request.FirstName;
            instructor.LastName = request.LastName;
            instructor.HireDate = request.HireDate;
            instructor.OfficeAssignment = request.HasAssignedOffice
                ? new OfficeAssignment {Location = request.Location}
                : null;

            await UpdateInstructorCourses(request.SelectedCourses, instructor);

            await _departmentsContext.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHs = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<Guid>
                (instructorToUpdate.CourseAssignments.Select(c => c.CourseExternalId));
            var courses = await _coursesRepository.GetAll();
            foreach (var course in courses)
                if (selectedCoursesHs.Contains(course.EntityId.ToString()))
                {
                    if (!instructorCourses.Contains(course.EntityId))
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment
                        {
                            InstructorId = instructorToUpdate.Id,
                            CourseExternalId = course.EntityId
                        });
                }
                else
                {
                    if (instructorCourses.Contains(course.EntityId))
                    {
                        var courseToRemove = instructorToUpdate
                                             .CourseAssignments
                                             .FirstOrDefault(i => i.CourseExternalId == course.EntityId);
                        
                        if (courseToRemove != null)
                            _departmentsContext.Remove(courseToRemove);
                    }
                }
        }
    }
}