namespace ContosoUniversity.Application.Contracts.Repositories.Writes;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Student;

using SharedKernel;

public interface IStudentsRwRepository : IRwRepository<Student>
{
    Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken = default);
}
