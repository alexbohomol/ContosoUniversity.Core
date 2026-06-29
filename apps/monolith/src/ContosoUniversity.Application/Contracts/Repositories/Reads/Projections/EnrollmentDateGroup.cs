namespace ContosoUniversity.Application.Contracts.Repositories.Reads.Projections;

using System;

public record EnrollmentDateGroup(DateTime EnrollmentDate, int StudentCount);
