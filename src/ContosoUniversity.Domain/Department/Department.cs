namespace ContosoUniversity.Domain.Department;

using System;

public class Department : IIdentifiable<Guid>
{
    private Department(
        string name,
        decimal budget,
        DateTime startDate,
        Guid? administratorId)
    {
        Name = name;
        Budget = budget;
        StartDate = startDate;
        AdministratorId = administratorId;
        ExternalId = Guid.NewGuid();
    }

    public string Name { get; private set; }

    public decimal Budget { get; private set; }

    public DateTime StartDate { get; private set; }

    public Guid? AdministratorId { get; private set; }

    public Guid ExternalId { get; }

    public static Department Create(string name, decimal budget, DateTime startDate, Guid? administratorId)
    {
        return new Department(name, budget, startDate, administratorId);
    }

    public void DisassociateAdministrator()
    {
        AdministratorId = null;
    }

    public void AssociateAdministrator(Guid instructorId)
    {
        AdministratorId = instructorId;
    }

    public void UpdateGeneralInfo(string name, decimal budget, DateTime startDate)
    {
        Name = name;
        Budget = budget;
        StartDate = startDate;
    }
}