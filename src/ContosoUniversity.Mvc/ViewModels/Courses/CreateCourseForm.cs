namespace ContosoUniversity.Mvc.ViewModels.Courses;

using System;
using System.Collections.Generic;

using Application.Services.Courses.Commands;

using Microsoft.AspNetCore.Mvc.Rendering;

public class CreateCourseForm : CreateCourseCommand
{
    public CreateCourseForm(
        IDictionary<Guid, string> departmentsNames)
    {
        DepartmentsSelectList = departmentsNames.ToSelectList();
    }

    public CreateCourseForm(
        CreateCourseCommand command,
        IDictionary<Guid, string> departmentsNames)
    {
        CourseCode = command.CourseCode;
        Title = command.Title;
        Credits = command.Credits;
        DepartmentId = command.DepartmentId;
        DepartmentsSelectList = departmentsNames.ToSelectList(command.DepartmentId);
    }

    public SelectList DepartmentsSelectList { get; }
}
