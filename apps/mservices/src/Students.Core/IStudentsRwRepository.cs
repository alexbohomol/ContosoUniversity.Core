namespace Students.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using ContosoUniversity.SharedKernel;

using Domain;

public interface IStudentsRwRepository : IRwRepository<Student>
{
    Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken = default);
}
