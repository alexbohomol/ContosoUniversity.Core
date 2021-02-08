namespace ContosoUniversity.ViewModels.Departments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DepartmentDetailsViewModel
    {
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public string Administrator { get; set; }

        public Guid ExternalId { get; set; }
    }
}