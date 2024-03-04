namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public class EditCourseForm : EditCourseRequest
{
    public EditCourseForm(
        EditCourseRequest request,
        int courseCode,
        IDictionary<Guid, string> departmentsNames)
    {
        Id = request.Id;
        Title = request.Title;
        Credits = request.Credits;
        DepartmentId = request.DepartmentId;

        CourseCode = courseCode;
        DepartmentsSelectList = departmentsNames.ToSelectList(request.DepartmentId);
    }

    public int CourseCode { get; }
    public SelectList DepartmentsSelectList { get; }
}
