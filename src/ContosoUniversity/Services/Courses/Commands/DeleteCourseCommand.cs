namespace ContosoUniversity.Services.Courses.Commands
{
    using System;

    using MediatR;

    public record DeleteCourseCommand(Guid Id) : IRequest;
}