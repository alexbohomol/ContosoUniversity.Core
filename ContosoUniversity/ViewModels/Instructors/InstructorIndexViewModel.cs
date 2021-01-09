namespace ContosoUniversity.ViewModels.Instructors
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class InstructorIndexViewModel
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IDictionary<Guid, string> AssignedCourses { get; set; }
        public IEnumerable<CourseListItemViewModel> SelectedInstructorCourseIds { get; set; }
        public IEnumerable<Enrollment> SelectedCourseEnrollments { get; set; }
    }
}