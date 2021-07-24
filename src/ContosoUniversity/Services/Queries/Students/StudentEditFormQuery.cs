namespace ContosoUniversity.Services.Queries.Students
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record StudentEditFormQuery(Guid Id) : IRequest<EditStudentForm>;
}