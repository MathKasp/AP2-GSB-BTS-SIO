using System.ComponentModel.DataAnnotations;
namespace newEmpty.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "L'identifiant est requis")]
    public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}