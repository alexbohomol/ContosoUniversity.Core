namespace ContosoUniversity.Mvc.ViewModels.Instructors;

using System;

public class CourseListItemViewModel
{
    public Guid Id { get; set; }
    public int CourseCode { get; set; }
    public string Title { get; set; }
    public string Department { get; set; }
    public string RowClass { get; set; }
}
