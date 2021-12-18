namespace ContosoUniversity.Application.Contracts.ReadModels;

using System;

public record EnrollmentDateGroup(DateTime EnrollmentDate, int StudentCount);