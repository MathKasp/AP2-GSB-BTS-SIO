using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;

public class Patient
{
    [Key]
    public int PatientId {get; set;}

    [Display(Name = "Nom")]
    public required string Nom_p { get; set; }

    [Display(Name = "Prénom")]
    public required string Prenom_p { get; set; }

    [Display(Name = "Sexe")]
    public required string Sexe_p { get; set; }

    [Display(Name = "Numéro de sécurité")]
    public required string Num_secu { get; set; }

    // relations
    
    [Display(Name = "Antécédent Médicaux")]
    public List<Antecedent> Antecedents { get; set; } = new(); // = new() veut dire que par défaut c'est une liste vide

    [Display(Name = "Allergies")]
    public List<Allergie> Allergies { get; set; } = new();

    public List<Ordonnance> Ordonnances { get; set; } = new();

}