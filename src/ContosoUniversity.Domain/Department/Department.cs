namespace ContosoUniversity.Domain.Department
{
    using System;

    public class Department : IAggregateRoot
    {
        public Department(
            string name, 
            decimal budget, 
            DateTime startDate, 
            Administrator administrator) 
            : this(
                name, 
                budget, 
                startDate, 
                administrator,
                Guid.NewGuid())
        {
        }

        public Department(
            string name, 
            decimal budget, 
            DateTime startDate, 
            Administrator administrator,
            Guid entityId)
        {
            Name = name;
            Budget = budget;
            StartDate = startDate;
            Administrator = administrator;
            EntityId = entityId;
        }

        public string Name { get; }

        public decimal Budget { get; }

        public DateTime StartDate { get; }

        public Administrator Administrator { get; private set; }

        public Guid EntityId { get; }

        public void DisassociateAdministrator()
        {
            Administrator = null;
        }
    }
}