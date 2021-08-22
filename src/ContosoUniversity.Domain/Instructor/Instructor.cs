namespace ContosoUniversity.Domain.Instructor
{
    using System;
    using System.Collections.Generic;

    public class Instructor : IIdentifiable<Guid>
    {
        public Instructor(
            string firstName,
            string lastName,
            DateTime hireDate,
            IList<Guid> courses,
            OfficeAssignment office)
            : this(
                firstName,
                lastName,
                hireDate,
                courses,
                office,
                Guid.NewGuid())
        { }

        public Instructor(
            string firstName, 
            string lastName,
            DateTime hireDate,
            IList<Guid> courses,
            OfficeAssignment office,
            Guid entityId)
        {
            FirstName = firstName;
            LastName = lastName;
            HireDate = hireDate;
            Courses = courses;
            Office = office;
            EntityId = entityId;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public DateTime HireDate { get; private set; }

        public IList<Guid> Courses { get; set; }

        public OfficeAssignment Office { get; set; }

        public Guid EntityId { get; }

        public void UpdatePersonalInfo(string firstName, string lastName, DateTime hireDate)
        {
            FirstName = firstName;
            LastName = lastName;
            HireDate = hireDate;
        }
    }
}