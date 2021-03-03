namespace ContosoUniversity.ViewModels.Courses
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Commands.Courses;

    public class EditCourseForm : EditCourseCommand
    {
        public int CourseCode { get; set; }
        public SelectList DepartmentsSelectList { get; set; }
    }
}