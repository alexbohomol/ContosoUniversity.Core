namespace ContosoUniversity.Services.Queries.Courses
{
    using System;

    using MediatR;

    using ViewModels.Courses;

    public class QueryCourseEditForm : IRequest<EditCourseForm>
    {
        public QueryCourseEditForm(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}