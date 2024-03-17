namespace ContosoUniversity.Mvc.ViewModels.Departments;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

public record EditDepartmentForm(Dictionary<Guid, string> InstructorNames)
{
    public EditDepartmentRequest Request { get; init; } = new();
    public SelectList InstructorsDropDown =>
        InstructorNames.ToSelectList(Request.AdministratorId.GetValueOrDefault());
}
