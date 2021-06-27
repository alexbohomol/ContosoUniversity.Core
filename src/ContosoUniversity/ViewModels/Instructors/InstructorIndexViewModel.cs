namespace ContosoUniversity.ViewModels.Instructors
{
    using System.Collections.Generic;

    using Data.Students.Models;

    public class InstructorIndexViewModel
    {
        public IEnumerable<InstructorListItemViewModel> Instructors { get; set; }
        public IEnumerable<CourseListItemViewModel> Courses { get; set; }
        public IEnumerable<EnrolledStudentViewModel> Students { get; set; }
    }
}