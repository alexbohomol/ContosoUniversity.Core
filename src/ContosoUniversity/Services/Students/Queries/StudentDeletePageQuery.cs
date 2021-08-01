namespace ContosoUniversity.Services.Students.Queries
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record StudentDeletePageQuery(Guid Id) : IRequest<StudentDeletePageViewModel>;
}