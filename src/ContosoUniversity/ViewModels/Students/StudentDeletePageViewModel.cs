namespace ContosoUniversity.ViewModels.Students
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentDeletePageViewModel
    {
        public const string ErrMsgSaveChanges = "Delete failed. Try again, and if the problem persists see your system administrator.";

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; init; }

        [Display(Name = "Last Name")]
        public string LastName { get; init; }

        [Display(Name = "First Name")]
        public string FirstMidName { get; init; }

        public Guid ExternalId { get; init; }

        public bool SaveChangesError { get; init; }
    }
}