namespace ContosoUniversity.Services.Queries.Students
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record QueryStudentDeletePage(Guid Id) : IRequest<StudentDeleteViewModel>;
}