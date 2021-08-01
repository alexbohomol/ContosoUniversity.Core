namespace ContosoUniversity.Services.Students.Queries
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record StudentEditFormQuery(Guid Id) : IRequest<EditStudentForm>;
}