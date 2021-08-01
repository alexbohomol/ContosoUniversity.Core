namespace ContosoUniversity.Services.Courses.Queries
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public record CourseDetailsQuery(Guid Id) : IRequest<CourseDetailsViewModel>;
}