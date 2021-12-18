namespace ContosoUniversity.Application.Contracts.Repositories.ReadOnly.Projections;

using System;

public record EnrollmentDateGroup(DateTime EnrollmentDate, int StudentCount);