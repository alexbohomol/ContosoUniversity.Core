namespace ContosoUniversity.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class ViewModelsExtensions
    {
        public static SelectList ToSelectList(
            this IDictionary<Guid, string> departments,
            Guid selectedDepartment = default)
        {
            return new(
                departments,
                "Key",
                "Value",
                selectedDepartment);
        }
    }
}