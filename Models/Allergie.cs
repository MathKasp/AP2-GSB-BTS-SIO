using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;

public class Allergie 
{
    [Key]
    public int AllergieId {get; set;}

    public required string Nom_Allergie {get; set;}

    //relations
    public List<Medicament> Medicaments {get; set;} = new();

    public List<Patient> Patients {get; set;} = new();
    
}