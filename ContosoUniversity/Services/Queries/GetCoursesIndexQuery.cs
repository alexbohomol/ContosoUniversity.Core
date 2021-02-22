namespace ContosoUniversity.Services.Queries
{
    using System.Collections.Generic;

    using MediatR;

    using ViewModels.Courses;

    public class GetCoursesIndexQuery : IRequest<List<CourseListItemViewModel>>
    {
    }
}