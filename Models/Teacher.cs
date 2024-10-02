using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;


public class Teacher
{

    [Required(ErrorMessage = "Teacher ID is required")]
    [Display(Name = "Teacher ID")]
    public int TeacherId { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [Display (Name = "Last Name")]
    public string? LastName { get; set; }

    [Display(Name = "Hiring Date")]
    [DataType(DataType.Date)]
    public DateTime HiringDate { get; set;}

    // public bool IsVeteran { get; set; }
    // public double GPA { get; set; }
    // public Major Major { get; set; }
}
