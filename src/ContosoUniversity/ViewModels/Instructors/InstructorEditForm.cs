namespace ContosoUniversity.ViewModels.Instructors
{
    using System.ComponentModel.DataAnnotations;

    using Services.Commands.Instructors;

    public class InstructorEditForm : InstructorEditCommand
    {
        public InstructorEditForm(
            InstructorEditCommand command, 
            AssignedCourseOption[] assignedCourses)
        {
            LastName = command.LastName;
            FirstName = command.FirstName;
            HireDate = command.HireDate;
            Location = command.Location;
            AssignedCourses = assignedCourses;
        }

        public InstructorEditForm()
        {
            
        }

        [Display(Name = "Assigned Courses")]
        public AssignedCourseOption[] AssignedCourses { get; set; }
    }
}