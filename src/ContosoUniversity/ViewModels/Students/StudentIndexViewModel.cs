namespace ContosoUniversity.ViewModels.Students
{
    using Domain.Contracts.Paging;

    public class StudentIndexViewModel
    {
        public string CurrentSort { get; init; }
        public string NameSortParm { get; init; }
        public string DateSortParm { get; init; }
        public string CurrentFilter { get; init; }
        public StudentListItemViewModel[] Items { get; init; }
        public PageInfo PageInfo { get; init; }
    }
}