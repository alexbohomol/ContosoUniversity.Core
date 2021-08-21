namespace ContosoUniversity.Domain.Instructor
{
    using System;

    public class Instructor : IIdentifiable<Guid>
    {
        public Instructor(
            string firstName, 
            string lastName,
            DateTime hireDate,
            Guid[] courses,
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

        public Guid[] Courses { get; set; }

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