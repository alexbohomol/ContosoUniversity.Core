namespace ContosoUniversity.ViewModels.Home
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; init; }
        
        public int StudentCount { get; init; }
    }
}