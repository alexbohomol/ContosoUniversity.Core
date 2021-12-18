namespace ContosoUniversity.Services.Students.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Contracts.Repositories.ReadOnly;
using Application.Contracts.Repositories.ReadOnly.Projections;
using Application.Exceptions;

using MediatR;

using ViewModels.Students;

public record GetStudentEditFormQuery(Guid Id) : IRequest<EditStudentForm>;

public class GetStudentEditFormQueryHandler : IRequestHandler<GetStudentEditFormQuery, EditStudentForm>
{
    private readonly IStudentsRoRepository _studentsRepository;

    public GetStudentEditFormQueryHandler(IStudentsRoRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }

    public async Task<EditStudentForm> Handle(GetStudentEditFormQuery request, CancellationToken cancellationToken)
    {
        Student student = await _studentsRepository.GetById(request.Id, cancellationToken);
        if (student == null)
            throw new EntityNotFoundException(nameof(student), request.Id);

        return new EditStudentForm
        {
            LastName = student.LastName,
            FirstName = student.FirstName,
            EnrollmentDate = student.EnrollmentDate,
            ExternalId = student.ExternalId
        };
    }
}