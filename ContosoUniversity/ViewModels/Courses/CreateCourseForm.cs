namespace ContosoUniversity.ViewModels.Courses
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Services.Commands.Courses;

    public class CreateCourseForm : CreateCourseCommand
    {
        public SelectList DepartmentsSelectList { get; set; }
    }
}