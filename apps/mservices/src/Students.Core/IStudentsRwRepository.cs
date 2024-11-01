namespace Students.Core;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain;

public interface IStudentsRwRepository
{
    Task<Student[]> GetStudentsEnrolledForCourses(Guid[] courseIds, CancellationToken cancellationToken = default);

    Task<Student> GetById(Guid entityId, CancellationToken cancellationToken = default);
    Task Save(Student entity, CancellationToken cancellationToken = default);
    Task Remove(Guid entityId, CancellationToken cancellationToken = default);
}
