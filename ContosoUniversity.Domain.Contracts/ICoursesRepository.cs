namespace ContosoUniversity.Domain.Contracts
{
    using System.Threading.Tasks;

    public interface ICoursesRepository : IRepository<Course>
    {
        Task<int> UpdateCourseCredits(int multiplier);
    }
}