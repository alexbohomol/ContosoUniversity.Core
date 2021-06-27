namespace ContosoUniversity.Services.Queries.Students
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record QueryStudentDetails(Guid Id) : IRequest<StudentDetailsViewModel>;
}