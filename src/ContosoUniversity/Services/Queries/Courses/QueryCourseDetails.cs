namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public record QueryCourseDetails(Guid Id) : IRequest<CourseDetailsViewModel>;
}