namespace ContosoUniversity.ViewModels.Instructors
{
    using System.Collections.Generic;

    using Data.Models;

    public class InstructorIndexViewModel
    {
        public IEnumerable<InstructorListItemViewModel> Instructors { get; set; }
        public IEnumerable<CourseListItemViewModel> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}