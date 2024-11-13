using System;
using System.ComponentModel.DataAnnotations;
using newEmpty.Models;

namespace newEmpty.ViewModel;

public class OrdonnanceViewModel
{
    public int OrdonnanceId {get; set;}

    [StringLength(100)]
    [Required(ErrorMessage = "La posologie est requise")]
    public string Posologie { get; set; }
 
    public string Duree_traitement {get; set;}

 
    [Required(ErrorMessage = "Instructions requises")]
    public string? Instructions_specifique { get; set; }
    
 
    [Required(ErrorMessage = "Veuillez sélectionner un patient")]
     public int? PatientId { get; set; }
 
     public Patient? Patient {get;set;}
 
 
     public Medecin? Medecin {get;set;}
    public List<Patient> Patients { get; set; } = new List<Patient>();
    public List<Medicament>? Medicaments { get; set; }
    public List<int> SelectedMedicamentId { get; set; } = new List<int>();
}
