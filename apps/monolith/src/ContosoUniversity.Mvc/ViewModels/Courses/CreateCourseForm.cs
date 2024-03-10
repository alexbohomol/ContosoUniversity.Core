namespace ContosoUniversity.Mvc.ViewModels.Courses;

using Microsoft.AspNetCore.Mvc.Rendering;

public record CreateCourseForm(SelectList DepartmentsSelectList)
{
    public CreateCourseRequest Request { get; init; }
};
