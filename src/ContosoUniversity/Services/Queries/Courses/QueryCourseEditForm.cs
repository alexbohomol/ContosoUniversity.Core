namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public record QueryCourseEditForm(Guid Id) : IRequest<EditCourseForm>;
}