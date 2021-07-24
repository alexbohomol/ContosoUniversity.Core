namespace ContosoUniversity.Services.Queries.Students
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record StudentDetailsQuery(Guid Id) : IRequest<StudentDetailsViewModel>;
}