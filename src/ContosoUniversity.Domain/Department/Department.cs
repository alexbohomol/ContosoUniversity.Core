namespace ContosoUniversity.Domain.Department
{
    using System;

    public class Department : IAggregateRoot
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
            Guid entityId)
        {
            Name = name;
            Budget = budget;
            StartDate = startDate;
            AdministratorId = administratorId;
            EntityId = entityId;
        }

        public string Name { get; }

        public decimal Budget { get; }

        public DateTime StartDate { get; }

        public Guid? AdministratorId { get; private set; }

        public Guid EntityId { get; }

        public void DisassociateAdministrator()
        {
            AdministratorId = null;
        }
    }
}