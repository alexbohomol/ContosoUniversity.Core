namespace ContosoUniversity.Domain
{
    using System;

    public class Department
    {
        private string _name;
        private decimal _budget;
        private DateTime _startDate;
        private Guid _administratorId;

        public Department(
            string name, 
            decimal budget, 
            DateTime startDate, 
            Guid administratorId) 
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
            Guid administratorId, 
            Guid externalId)
        {
            Name = name;
            Budget = budget;
            StartDate = startDate;
            AdministratorId = administratorId;
            ExternalId = externalId;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public decimal Budget
        {
            get => _budget;
            set => _budget = value;
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        public Guid AdministratorId
        {
            get => _administratorId;
            set => _administratorId = value;
        }

        public Guid ExternalId { get; }
    }
}