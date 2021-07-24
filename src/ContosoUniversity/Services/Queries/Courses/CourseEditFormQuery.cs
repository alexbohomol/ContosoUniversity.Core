namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public record CourseEditFormQuery(Guid Id) : IRequest<CourseEditForm>;
}