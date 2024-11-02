namespace ContosoUniversity.Messaging.Contracts.Commands;

using System;

using MediatR;

public record CreateStudentCommand(
    DateTime EnrollmentDate,
    string LastName,
    string FirstName) : IRequest;
