namespace ContosoUniversity.Services.Students.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using ViewModels.Students;

    public record GetStudentEditFormQuery(Guid Id) : IRequest<EditStudentForm>;
    
    public class GetStudentEditFormQueryHandler : IRequestHandler<GetStudentEditFormQuery, EditStudentForm>
    {
        private readonly IStudentsRepository _studentsRepository;

        public GetStudentEditFormQueryHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task<EditStudentForm> Handle(GetStudentEditFormQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentsRepository.GetById(request.Id);
            if (student == null)
                throw new EntityNotFoundException(nameof(student), request.Id);

            return new EditStudentForm
            {
                LastName = student.LastName,
                FirstName = student.FirstName,
                EnrollmentDate = student.EnrollmentDate,
                ExternalId = student.EntityId
            };
        }
    }
}