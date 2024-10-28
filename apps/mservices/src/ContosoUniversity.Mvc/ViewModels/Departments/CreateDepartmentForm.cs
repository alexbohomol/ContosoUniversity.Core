namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public record CreateDepartmentForm(Dictionary<Guid, string> InstructorNames)
{
    public CreateDepartmentRequest Request { get; init; } = new();
    public SelectList InstructorsDropDown => InstructorNames.ToSelectList();
}
