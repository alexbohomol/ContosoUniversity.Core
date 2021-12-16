namespace ContosoUniversity.Domain.Contracts;

using System;
using System.Threading;
using System.Threading.Tasks;

using Student;

public interface IStudentsRepository : IStudentsRwRepository, IStudentsRoRepository
{
    [Obsolete("Temporarily hides original `GetById` methods from both bases")]
    new Task<Student> GetById(Guid entityId, CancellationToken cancellationToken = default);

    [Obsolete("Temporarily hides original `GetStudentsEnrolledForCourses` methods from both bases")]
    new Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken = default);
}