namespace ContosoUniversity.Services.Queries.Students
{
    using System;

    using MediatR;

    using ViewModels.Students;

    public record StudentDeletePageQuery(Guid Id) : IRequest<StudentDeletePageViewModel>;
}