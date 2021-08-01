namespace ContosoUniversity.Services.Courses.Queries
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public record CourseEditFormQuery(Guid Id) : IRequest<CourseEditForm>;
}