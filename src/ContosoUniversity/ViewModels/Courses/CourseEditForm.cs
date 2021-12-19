namespace ContosoUniversity.ViewModels.Courses;

using System;
using System.Collections.Generic;

using Application.Services.Courses.Commands;

using Microsoft.AspNetCore.Mvc.Rendering;

public class CourseEditForm : EditCourseCommand
{
    public CourseEditForm(
        EditCourseCommand command,
        int courseCode,
        IDictionary<Guid, string> departmentsNames)
    {
        Id = command.Id;
        Title = command.Title;
        Credits = command.Credits;
        DepartmentId = command.DepartmentId;

        CourseCode = courseCode;
        DepartmentsSelectList = departmentsNames.ToSelectList(command.DepartmentId);
    }

    public int CourseCode { get; }
    public SelectList DepartmentsSelectList { get; }
}