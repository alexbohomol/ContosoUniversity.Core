namespace ContosoUniversity.Application.ApiClients;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SharedKernel.Paging;

public interface IStudentsApiClient
{
    Task<Student[]> GetStudentsEnrolledForCourses(
        Guid[] courseIds,
        CancellationToken cancellationToken);

    Task<EnrollmentDateGroup[]> GetEnrollmentDateGroups(CancellationToken cancellationToken);

    Task<PagedResult<Student>> Search(
        SearchRequest searchRequest,
        OrderRequest orderRequest,
        PageRequest pageRequest,
        CancellationToken cancellationToken);

    Task<Student> GetById(Guid externalId, CancellationToken cancellationToken);
}

public enum Grade
{
    A,
    B,
    C,
    D,
    F
}

public record Enrollment(
    Guid StudentId,
    Guid CourseId,
    Grade? Grade);

public record Student(
    string LastName,
    string FirstName,
    DateTime EnrollmentDate,
    Guid ExternalId)
{
    public IList<Enrollment> Enrollments { get; set; }
    public string FullName => $"{FirstName}, {LastName}";
}

public record EnrollmentDateGroup(DateTime EnrollmentDate, int StudentCount);
