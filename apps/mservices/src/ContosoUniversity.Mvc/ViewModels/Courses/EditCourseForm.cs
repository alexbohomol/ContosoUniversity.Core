namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public record EditCourseForm(int CourseCode, Dictionary<Guid, string> DepartmentsReference)
{
    public EditCourseRequest Request { get; init; } = new();
    public SelectList DepartmentsSelectList => DepartmentsReference.ToSelectList(Request.DepartmentId);
}
