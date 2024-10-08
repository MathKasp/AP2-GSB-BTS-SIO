using System.ComponentModel.DataAnnotations;
namespace newEmpty.ViewModel;

public class LoginViewModel
{

    [Required(ErrorMessage = "The mail is required.")]
    public required string Email {get; set;}

    [Required(ErrorMessage = "The Username field is required.")]
    public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}