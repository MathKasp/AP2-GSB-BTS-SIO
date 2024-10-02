using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;

public class Patient
{
    [Key]
    public int PatientId {get; set;}
    public required string Nom_p { get; set; }
    public required string Prenom_p { get; set; }
    public required string Sexe_p { get; set; }
    public required string Num_secu { get; set; }

    // relations
    public List<Antecedent> Antecedents { get; set; } = new(); // = new() veut dire que par défaut c'est une liste vide
    public List<Allergie> Allergies { get; set; } = new();
    public List<Ordonnance> Ordonnances { get; set; } = new();

}