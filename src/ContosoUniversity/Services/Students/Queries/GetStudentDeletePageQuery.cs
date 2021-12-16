namespace ContosoUniversity.Services.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;
using Domain.Contracts.Exceptions;
using Domain.Student;

using MediatR;

using ViewModels.Students;

public record GetStudentDeletePageQuery(Guid Id) : IRequest<StudentDeletePageViewModel>;

public class GetStudentDeletePageQueryHandler : IRequestHandler<GetStudentDeletePageQuery, StudentDeletePageViewModel>
{
    private readonly IStudentsRoRepository _studentsRepository;

    public GetStudentDeletePageQueryHandler(IStudentsRoRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task<StudentDeletePageViewModel> Handle(GetStudentDeletePageQuery request,
        CancellationToken cancellationToken)
    {
        Student student = await _studentsRepository.GetById(request.Id, cancellationToken);
        if (student == null)
            throw new EntityNotFoundException(nameof(student), request.Id);

        return new StudentDeletePageViewModel
        {
            LastName = student.LastName,
            FirstMidName = student.FirstName,
            EnrollmentDate = student.EnrollmentDate,
            ExternalId = student.ExternalId
        };
    }
}