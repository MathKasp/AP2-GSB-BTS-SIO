using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.RateLimiting;

namespace newEmpty.Models;

public class Ordonnance
{
    [Key]
    public int OrdonnanceId {get; set;}

    public required string Posologie {get; set;}

    public required string Duree_traitement {get; set;}

    public required string Instructions_specifique {get; set;}

    // relations
    public int MedecinId {get; set;}
    public required Medecin Medecin {get; set;}

    public int PatientId {get; set;}
    public required Patient Patient {get; set;}

    public List<Medicament> Medicaments {get; set;} = new ();


}