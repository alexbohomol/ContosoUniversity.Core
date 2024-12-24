namespace Courses.Core.Handlers.Commands;

using MediatR;

public record UpdateCoursesCreditsCommand(int Multiplier) : IRequest<int>;
