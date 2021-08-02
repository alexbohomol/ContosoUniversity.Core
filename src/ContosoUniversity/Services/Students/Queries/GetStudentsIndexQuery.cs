namespace ContosoUniversity.Services.Students.Queries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Paging;
    using Domain.Student;

    using MediatR;

    using ViewModels.Students;

    public class GetStudentsIndexQuery : IRequest<StudentIndexViewModel>
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageNumber { get; set; }
    }
    
    public class GetStudentsIndexQueryHandler : IRequestHandler<GetStudentsIndexQuery, StudentIndexViewModel>
    {
        private readonly IStudentsRepository _studentsRepository;

        public GetStudentsIndexQueryHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }
        
        public async Task<StudentIndexViewModel> Handle(GetStudentsIndexQuery request, CancellationToken cancellationToken)
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