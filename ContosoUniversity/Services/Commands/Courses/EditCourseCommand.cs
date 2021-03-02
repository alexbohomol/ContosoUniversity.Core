namespace ContosoUniversity.Services.Commands.Courses
{
    using System;

    using MediatR;

    public class EditCourseCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
    }
}