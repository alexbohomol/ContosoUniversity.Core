namespace ContosoUniversity.ViewModels.Instructors
{
    using System.ComponentModel.DataAnnotations;

    using Services.Instructors.Commands;

    public class CreateInstructorForm : CreateInstructorCommand
    {
        public CreateInstructorForm(CreateInstructorCommand command, AssignedCourseOption[] assignedCourses)
        {
            LastName = command.LastName;
            FirstName = command.FirstName;
            HireDate = command.HireDate;
            Location = command.Location;
            AssignedCourses = assignedCourses;
        }

        public CreateInstructorForm()
        {
            
        }

        [Display(Name = "Assigned Courses")]
        public AssignedCourseOption[] AssignedCourses { get; set; }
    }
}