using System;
using System.ComponentModel.DataAnnotations;
using newEmpty.Models;

namespace newEmpty.ViewModel;

public class PatientEditViewModel
{
    public required Patient? Patient { get; set; }
    public List<Antecedent>? Antecedents { get; set; }
    public List<Allergie>? Allergies { get; set; }
    public List<int> SelectedAntecedentIds { get; set; } = new List<int>();
    public List<int> SelectedAllergieIds { get; set; } = new List<int>();

}