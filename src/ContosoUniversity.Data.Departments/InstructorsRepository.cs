namespace ContosoUniversity.Data.Departments;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Domain.Contracts;

using Microsoft.EntityFrameworkCore;

using Models;

using Instructor = Domain.Instructor.Instructor;
using OfficeAssignment = Domain.Instructor.OfficeAssignment;

public class InstructorsRepository : EfRepository<Instructor, Models.Instructor>, IInstructorsRepository
{
    public InstructorsRepository(DepartmentsContext dbContext)
        : base(
            dbContext,
            new[]
            {
                nameof(Models.Instructor.OfficeAssignment),
                nameof(Models.Instructor.CourseAssignments)
            })
    {
    }

    public Task<Dictionary<Guid, string>> GetInstructorNamesReference(CancellationToken cancellationToken = default)
    {
        return DbSet
            .AsNoTracking()
            .ToDictionaryAsync(
                x => x.ExternalId,
                x => x.FullName,
                cancellationToken);
    }

    protected override Instructor ToDomainEntity(Models.Instructor dataModel)
    {
        return new Instructor(
            dataModel.FirstMidName,
            dataModel.LastName,
            dataModel.HireDate,
            dataModel.CourseAssignments.Select(x => x.CourseExternalId).ToList(),
            new OfficeAssignment(dataModel.OfficeAssignment?.Location),
            dataModel.ExternalId);
    }

    protected override void MapDomainEntityOntoDataEntity(Instructor source, Models.Instructor target)
    {
        target.FirstMidName = source.FirstName;
        target.LastName = source.LastName;
        target.HireDate = source.HireDate;
        target.ExternalId = source.ExternalId;

        target.CourseAssignments = source.Courses.Select(x => new CourseAssignment
        {
            InstructorId = target.Id,
            CourseExternalId = x
        }).ToList();

        target.OfficeAssignment = string.IsNullOrWhiteSpace(source.Office?.Title)
            ? null
            : new Models.OfficeAssignment
            {
                Location = source.Office.Title
            };
    }
}