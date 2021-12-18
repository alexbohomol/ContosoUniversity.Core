namespace ContosoUniversity.Application.Contracts.Repositories.ReadWrite;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Student;

public interface IStudentsRwRepository : IRwRepository<Student>
{
    Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken = default);
}