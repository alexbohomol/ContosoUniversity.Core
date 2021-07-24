namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public record CourseDetailsQuery(Guid Id) : IRequest<CourseDetailsViewModel>;
}