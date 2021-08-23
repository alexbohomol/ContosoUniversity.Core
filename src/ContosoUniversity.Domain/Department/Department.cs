namespace ContosoUniversity.Domain.Department
{
    using System;

    public class Department : IIdentifiable<Guid>
    {
        public Department(
            string name, 
            decimal budget, 
            DateTime startDate, 
            Guid? administratorId) 
            : this(
                name, 
                budget, 
                startDate, 
                administratorId,
                Guid.NewGuid())
        {
        }

        public Department(
            string name, 
            decimal budget, 
            DateTime startDate, 
            Guid? administratorId,
            Guid externalId)
        {
            Name = name;
            Budget = budget;
            StartDate = startDate;
            AdministratorId = administratorId;
            ExternalId = externalId;
        }

        public string Name { get; private set; }

        public decimal Budget { get; private set; }

        public DateTime StartDate { get; private set; }

        public Guid? AdministratorId { get; private set; }

        public Guid ExternalId { get; }

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
}