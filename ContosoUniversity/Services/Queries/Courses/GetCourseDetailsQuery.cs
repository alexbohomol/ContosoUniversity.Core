namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public class GetCourseDetailsQuery : IRequest<CourseDetailsViewModel>
    {
        public GetCourseDetailsQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}