namespace ContosoUniversity.Services.Queries.Courses
{
    using System.Collections.Generic;

    using MediatR;

    using ViewModels.Courses;

    public record CoursesIndexQuery : IRequest<List<CourseListItemViewModel>>;
}