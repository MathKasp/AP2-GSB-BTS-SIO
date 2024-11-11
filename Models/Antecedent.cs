using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;

public class Antecedent 
{
    [Key]
    public int AntecedentId {get; set;}

    [Required(ErrorMessage = "L'Antecedent doit posséder un Nom")]
    public required string Nom_Antecedent {get; set;}

    //relations
    public List<Medicament> Medicaments {get; set;} = new();

    public List<Patient> Patients {get; set;} = new();
    
}