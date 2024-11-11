using System;
using System.ComponentModel.DataAnnotations;
using newEmpty.Models;

namespace newEmpty.ViewModel;

public class AntecedentEditViewModel
{
    public required Antecedent? Antecedent { get; set; }
    public List<Medicament>? Medicaments { get; set; }
    public List<int> SelectedMedicamentIds { get; set; } = new List<int>();

}