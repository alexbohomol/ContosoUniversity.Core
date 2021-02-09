namespace ContosoUniversity.ViewModels.Students
{
    using Models;

    public class StudentIndexViewModel
    {
        public string CurrentSort { get; init; }
        public string NameSortParm { get; init; }
        public string DateSortParm { get; init; }
        public string CurrentFilter { get; init; }
        public PaginatedList<Student, StudentListItemViewModel> Page { get; init; }
    }
}