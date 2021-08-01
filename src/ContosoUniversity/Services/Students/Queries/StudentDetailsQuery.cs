namespace ContosoUniversity.Services.Students.Queries
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record StudentDetailsQuery(Guid Id) : IRequest<StudentDetailsViewModel>;
}