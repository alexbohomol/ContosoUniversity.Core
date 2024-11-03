namespace Courses.Core.Handlers.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

internal class UpdateCoursesCreditsCommandHandler(
    ICoursesRwRepository repository)
    : IRequestHandler<UpdateCoursesCreditsCommand, int>
{
    public Task<int> Handle(UpdateCoursesCreditsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return repository.UpdateCourseCredits(request.Multiplier, cancellationToken);
    }
}
