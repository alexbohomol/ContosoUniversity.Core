namespace ContosoUniversity.Services.Handlers.Students
{
    using System.Threading;
    using System.Threading.Tasks;

    using Domain.Contracts;
    using Domain.Contracts.Exceptions;

    using MediatR;

    using Queries.Students;

    using ViewModels.Students;

    public class StudentEditFormQueryHandler : IRequestHandler<StudentEditFormQuery, EditStudentForm>
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentEditFormQueryHandler(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        public async Task<EditStudentForm> Handle(StudentEditFormQuery request, CancellationToken cancellationToken)
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