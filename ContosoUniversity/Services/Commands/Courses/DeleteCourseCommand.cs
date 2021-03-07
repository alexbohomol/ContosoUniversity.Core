namespace ContosoUniversity.Services.Commands.Courses
{
    using System;

    using MediatR;

    public record DeleteCourseCommand(Guid Id) : IRequest;
}