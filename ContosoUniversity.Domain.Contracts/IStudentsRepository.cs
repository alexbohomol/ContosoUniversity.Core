namespace ContosoUniversity.Domain.Contracts
{
    using System.Threading.Tasks;

    using Student;

    public interface IStudentsRepository : IRepository<Student>
    {
        Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups();
    }
}