namespace ContosoUniversity.Data.Courses.Models;

using System;
using System.ComponentModel.DataAnnotations;

using Domain;

public class Course : IIdentifiable<Guid>
{
    public int Id { get; set; }

    public int CourseCode { get; set; }

    [StringLength(50, MinimumLength = 3)] public string Title { get; set; }

    [Range(0, 5)] public int Credits { get; set; }

    public Guid DepartmentExternalId { get; set; }
    public Guid ExternalId { get; set; }
}