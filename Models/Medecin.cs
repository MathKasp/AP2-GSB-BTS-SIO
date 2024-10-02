using System;
using System.ComponentModel.DataAnnotations;

namespace newEmpty.Models;

public class Medecin
{
    [Key]
    public int MedecinId {get; set;}

    public required string Nom_m {get; set;}

    public required string Prenom_m {get; set;}

    public DateTime Date_naissance_m {get; set;}

    public required string Identifiant_m {get; set;}

    public required string MotDePasse_m {get; set;}

    // relations
    public List<Ordonnance> Ordonnances {get; set;} = new();


}