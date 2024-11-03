namespace Courses.Core.Handlers;

using MediatR;

public record UpdateCoursesCreditsCommand(int Multiplier) : IRequest<int>;
