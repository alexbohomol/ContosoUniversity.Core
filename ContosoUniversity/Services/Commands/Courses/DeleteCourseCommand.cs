namespace ContosoUniversity.Services.Commands.Courses
{
    using System;

    using MediatR;

    public class DeleteCourseCommand : IRequest
    {
        public DeleteCourseCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}