using System;
using System.ComponentModel.DataAnnotations;
using newEmpty.Models;


namespace newEmpty.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Le Nom est requis")]
    public required string Nom_m { get; set; }

    [Required(ErrorMessage = "Le Prenom est requis")]
    public required string Prenom_m { get; set; }

    [Required(ErrorMessage = "L'identifiant est requis")]
    public required string Identifiant_m { get; set; }

    [Required]
    public required string Password { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
