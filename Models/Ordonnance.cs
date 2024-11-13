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

    public string? Instructions_specifique {get; set;}

    // relations
    public required string MedecinId {get; set;}
    public  Medecin? Medecin {get; set;}

    public required int PatientId {get; set;}
    public Patient? Patient {get; set;}

    public List<Medicament> Medicaments {get; set;} = new ();


}