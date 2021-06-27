namespace ContosoUniversity.Services.Queries.Students
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record QueryStudentEditForm(Guid Id) : IRequest<EditStudentForm>;
}