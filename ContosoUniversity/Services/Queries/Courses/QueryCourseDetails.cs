namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public class QueryCourseDetails : IRequest<CourseDetailsViewModel>
    {
        public QueryCourseDetails(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}