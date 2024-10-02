using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;

public class Medicament
{
    [Key]
    public int MedicamentId { get; set;}

    public required string Nom_med {get; set;}

    public required string Contre_indication {get; set;}

    //relations
    public List<Allergie> Allergies { get; set;} = new();

    public List<Antecedent> Antecedents {get; set;} = new();

    public List<Ordonnance> Ordonnances {get; set;} = new();
    
}