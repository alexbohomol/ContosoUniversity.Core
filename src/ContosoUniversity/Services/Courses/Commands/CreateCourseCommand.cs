namespace ContosoUniversity.Services.Courses.Commands
{
    using System;

    using MediatR;

    public class CreateCourseCommand : IRequest
    {
        public int CourseCode { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
    }
}