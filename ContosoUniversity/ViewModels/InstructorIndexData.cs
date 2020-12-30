namespace ContosoUniversity.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IDictionary<Guid, Course> CoursesReference { get; set; }
        public IEnumerable<Course> SelectedInstructorCourses { get; set; }
        public IEnumerable<Enrollment> SelectedCourseEnrollments { get; set; }
    }
}