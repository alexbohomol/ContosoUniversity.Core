namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public record CreateCourseForm(Dictionary<Guid, string> DepartmentsReference)
{
    public CreateCourseRequest Request { get; init; } = new();
    public SelectList DepartmentsSelectList => DepartmentsReference.ToSelectList();
}
