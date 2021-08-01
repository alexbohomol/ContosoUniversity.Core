namespace ContosoUniversity.Services.Students.Queries
{
    using MediatR;

    using ViewModels.Students;

    public class StudentsIndexQuery : IRequest<StudentIndexViewModel>
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageNumber { get; set; }
    }
}