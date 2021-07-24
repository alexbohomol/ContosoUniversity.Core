namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;

    using Domain.Contracts;
    using Domain.Contracts.Paging;
    using Domain.Student;

    using MediatR;

    using Queries.Students;

    using ViewModels.Students;

    public class StudentsIndexQueryHandler : IRequestHandler<StudentsIndexQuery, StudentIndexViewModel>
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentsIndexQueryHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }
        
        public async Task<StudentIndexViewModel> Handle(StudentsIndexQuery request, CancellationToken cancellationToken)
        {
            if (request.SearchString != null)
            {
                request.PageNumber = 1;
            }
            else
            {
                request.SearchString = request.CurrentFilter;
            }

            (Student[] students, PageInfo pageInfo) = await _studentsRepository.Search(
                new SearchRequest(request.SearchString), 
                new OrderRequest(request.SortOrder), 
                new PageRequest(request.PageNumber ?? 1, PageSize: 3));

            return new StudentIndexViewModel
            {
                CurrentSort = request.SortOrder,
                NameSortParm = string.IsNullOrWhiteSpace(request.SortOrder) ? "name_desc" : string.Empty,
                DateSortParm = request.SortOrder == "Date" ? "date_desc" : "Date",
                CurrentFilter = request.SearchString,
                Items = students.Select(s => new StudentListItemViewModel
                {
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    EnrollmentDate = s.EnrollmentDate,
                    ExternalId = s.EntityId
                }).ToArray(),
                PageInfo = pageInfo
            };
        }
    }
}