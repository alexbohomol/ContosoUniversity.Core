namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;

    using Data.Students;
    using Data.Students.Models;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Queries.Students;

    using ViewModels;
    using ViewModels.Students;

    public class QueryStudentsIndexHandler : IRequestHandler<QueryStudentsIndex, StudentIndexViewModel>
    {
        private readonly StudentsContext _studentsContext;

        public QueryStudentsIndexHandler(StudentsContext studentsContext)
        {
            _studentsContext = studentsContext;
        }
        
        public async Task<StudentIndexViewModel> Handle(QueryStudentsIndex request, CancellationToken cancellationToken)
        {
            if (request.SearchString != null)
            {
                request.PageNumber = 1;
            }
            else
            {
                request.SearchString = request.CurrentFilter;
            }

            var students = from s in _studentsContext.Students select s;
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                students = students.Where(s => s.LastName.Contains(request.SearchString)
                                               || s.FirstMidName.Contains(request.SearchString));
            }

            switch (request.SortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            var page = await PaginatedList<Student, StudentListItemViewModel>.CreateAsync(
                students.AsNoTracking(),
                request.PageNumber ?? 1,
                3,
                s => new StudentListItemViewModel
                {
                    LastName = s.LastName,
                    FirstName = s.FirstMidName,
                    EnrollmentDate = s.EnrollmentDate,
                    ExternalId = s.ExternalId
                });

            return new StudentIndexViewModel
            {
                CurrentSort = request.SortOrder,
                NameSortParm = string.IsNullOrWhiteSpace(request.SortOrder) ? "name_desc" : string.Empty,
                DateSortParm = request.SortOrder == "Date" ? "date_desc" : "Date",
                CurrentFilter = request.SearchString,
                Page = page
            };
        }
    }
}