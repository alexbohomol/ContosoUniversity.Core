namespace ContosoUniversity.Domain.Instructor
{
    using System;

    public class Instructor : IIdentifiable<Guid>
    {
        public Instructor(
            string firstName,
            string lastName,
            DateTime hireDate,
            CourseAssignment[] courses,
            OfficeAssignment office)
            : this(
                firstName, 
                lastName, 
                hireDate, 
                courses, 
                office,
                Guid.NewGuid())
        {
        }

        public Instructor(
            string firstName, 
            string lastName,
            DateTime hireDate,
            CourseAssignment[] courses,
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

        public string FirstName { get; }
        
        public string LastName { get; }

        public DateTime HireDate { get; }

        public CourseAssignment[] Courses { get; }
        
        public OfficeAssignment Office { get; }
        
        public Guid EntityId { get; }
    }
}