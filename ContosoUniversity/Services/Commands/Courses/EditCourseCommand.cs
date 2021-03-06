namespace ContosoUniversity.Services.Commands.Courses
{
    using System;

    using Domain;

    using MediatR;

    public class EditCourseCommand : IRequest
    {
        public EditCourseCommand(Course course)
        {
            Id = course.EntityId;
            Title = course.Title;
            Credits = course.Credits;
            DepartmentId = course.DepartmentId;
        }

        public EditCourseCommand()
        {
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public Guid DepartmentId { get; set; }
    }
}