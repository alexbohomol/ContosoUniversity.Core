namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public class CreateCourseForm : CreateCourseRequest
{
    public CreateCourseForm(
        IDictionary<Guid, string> departmentsNames)
    {
        DepartmentsSelectList = departmentsNames.ToSelectList();
    }

    public CreateCourseForm(
        CreateCourseRequest request,
        IDictionary<Guid, string> departmentsNames)
    {
        CourseCode = request.CourseCode;
        Title = request.Title;
        Credits = request.Credits;
        DepartmentId = request.DepartmentId;
        DepartmentsSelectList = departmentsNames.ToSelectList(request.DepartmentId);
    }

    public SelectList DepartmentsSelectList { get; }
}
