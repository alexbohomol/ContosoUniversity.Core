namespace ContosoUniversity.Application.Services.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Contracts.Repositories.ReadOnly;
using Contracts.Repositories.ReadOnly.Projections;

using Exceptions;

using MediatR;

public record GetStudentProjectionQuery(Guid Id) : IRequest<Student>;

public class GetStudentEditFormQueryHandler : IRequestHandler<GetStudentProjectionQuery, Student>
{
    private readonly IStudentsRoRepository _studentsRepository;

    public GetStudentEditFormQueryHandler(IStudentsRoRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task<Student> Handle(
        GetStudentProjectionQuery request,
        CancellationToken cancellationToken)
    {
        Student student = await _studentsRepository.GetById(request.Id, cancellationToken);
        if (student == null)
            throw new EntityNotFoundException(nameof(student), request.Id);

        return student;
    }
}