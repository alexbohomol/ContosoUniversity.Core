namespace ContosoUniversity.Domain.Student
{
    using System;

    public record EnrollmentDateGroup(DateTime EnrollmentDate, int StudentCount);
}