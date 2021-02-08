namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DepartmentDeleteViewModel
    {
        public const string ConcurrencyErrorMessage = "The record you attempted to delete was modified by another user after you got the original values. The delete operation was canceled and the current values in the database have been displayed. If you still want to delete this record, click the Delete button again. Otherwise click the Back to List hyperlink.";

        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public string Administrator { get; set; }

        public Guid ExternalId { get; set; }

        public byte[] RowVersion { get; set; }
        public bool ConcurrencyError { get; set; }
    }
}