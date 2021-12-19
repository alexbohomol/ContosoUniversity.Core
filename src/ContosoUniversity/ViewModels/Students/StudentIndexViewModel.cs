namespace ContosoUniversity.ViewModels.Students;

using System;
using System.Linq;

using Application.Contracts.Repositories.ReadOnly.Paging;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Services.Students.Queries;

public class StudentIndexViewModel
{
    public StudentIndexViewModel(GetStudentsIndexQuery request, PageInfo pageInfo, Student[] students)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        ArgumentNullException.ThrowIfNull(pageInfo, nameof(pageInfo));
        ArgumentNullException.ThrowIfNull(students, nameof(students));

        CurrentSort = request.SortOrder;
        NameSortParm = string.IsNullOrWhiteSpace(request.SortOrder) ? "name_desc" : string.Empty;
        DateSortParm = request.SortOrder == "Date" ? "date_desc" : "Date";
        CurrentFilter = request.SearchString;

        Items = students.Select(s => new StudentListItemViewModel
        {
            LastName = s.LastName,
            FirstName = s.FirstName,
            EnrollmentDate = s.EnrollmentDate,
            ExternalId = s.ExternalId
        }).ToArray();

        PageInfo = pageInfo;
    }

    public string CurrentSort { get; }
    public string NameSortParm { get; }
    public string DateSortParm { get; }
    public string CurrentFilter { get; }
    public StudentListItemViewModel[] Items { get; }
    public PageInfo PageInfo { get; }
}