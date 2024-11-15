using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;

namespace newEmpty.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;

        // Controleur, injection de dependance
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]

        #region INDEX
        public IActionResult Index()
        {
            List<Patient> patients = new List<Patient>();
            patients = _context.Patients.ToList();
            return View(patients);
        }
        #endregion


        #region ADD
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new PatientViewModel { };

            model.Antecedents = await _context.Antecedents.ToListAsync();
            model.Allergies = await _context.Allergies.ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> AddConfirmed(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Patient.Allergies.Clear();
                if (model.SelectedAllergieIds != null)
                {
                    var selectedAllergies = await _context.Allergies
                        .Where(a => model.SelectedAllergieIds.Contains(a.AllergieId))
                        .ToListAsync();
                    foreach (var allergie in selectedAllergies)
                    {
                        model.Patient.Allergies.Add(allergie);
                    }
                }
                model.Patient.Antecedents.Clear();
                if (model.SelectedAntecedentIds != null)
                {
                    var selectedAntecedents = await _context.Antecedents
                        .Where(a => model.SelectedAntecedentIds.Contains(a.AntecedentId))
                        .ToListAsync();
                    foreach (var antecedent in selectedAntecedents)
                    {
                        model.Patient.Antecedents.Add(antecedent);
                    }
                }
                _context.Patients.Add(model.Patient);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            //return View(model);
            return RedirectToAction("Add");
        }
        #endregion


        #region REMOVE
        [HttpGet]
        public IActionResult Remove(int id)
        {
            Patient? patient = _context.Patients.FirstOrDefault(s => s.PatientId == id);
            return View(patient);
        }

        [HttpPost]
        public IActionResult RemoveConfirm(int PatientId)
        {
            List<Patient> patients = new List<Patient>();
            patients = _context.Patients.ToList();

            Patient? dpatient = patients.FirstOrDefault(s => s.PatientId == PatientId);

            if (dpatient != null)
            {
                _context.Patients.Remove(dpatient);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        #endregion


        #region EDIT
        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Antecedents)
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            // verifier si les allergies et/ou les antecedents sont nulls
            var viewModel = new PatientViewModel
            {
                Patient = patient,
                Antecedents = await _context.Antecedents.ToListAsync(),
                Allergies = await _context.Allergies.ToListAsync(),
                SelectedAntecedentIds = patient.Antecedents.Select(a => a.AntecedentId).ToList() ?? new List<int>(),
                SelectedAllergieIds = patient.Allergies.Select(a => a.AllergieId).ToList() ?? new List<int>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> Edit(int id, PatientViewModel viewModel)
        {
            if (id != viewModel.Patient.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _context.Patients
                        .Include(p => p.Antecedents)
                        .Include(p => p.Allergies)
                        .FirstOrDefaultAsync(p => p.PatientId == id);

                    if (patient == null)
                    {
                        return NotFound();
                    }

                    // Les propriétés saisis par l'utilisateur son passé dans la var patient
                    patient.Nom_p = viewModel.Patient.Nom_p;
                    patient.Prenom_p = viewModel.Patient.Prenom_p;
                    patient.Sexe_p = viewModel.Patient.Sexe_p;
                    patient.Num_secu = viewModel.Patient.Num_secu;

                    // mise a jour des allergies du patient
                    patient.Allergies.Clear();
                    if (viewModel.SelectedAllergieIds != null)
                    {
                        var selectedAllergies = await _context.Allergies
                            .Where(a => viewModel.SelectedAllergieIds.Contains(a.AllergieId))
                            .ToListAsync();
                        foreach (var allergie in selectedAllergies)
                        {
                            patient.Allergies.Add(allergie);
                        }
                    }

                    // mise a jour des antecedents du patient
                    patient.Antecedents.Clear();
                    if (viewModel.SelectedAntecedentIds != null)
                    {
                        var selectedAntecedents = await _context.Antecedents
                            .Where(a => viewModel.SelectedAntecedentIds.Contains(a.AntecedentId))
                            .ToListAsync();
                        foreach (var antecedent in selectedAntecedents)
                        {
                            patient.Antecedents.Add(antecedent);
                        }
                    }
                    _context.Entry(patient).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(viewModel.Patient.PatientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            viewModel.Antecedents = await _context.Antecedents.ToListAsync();
            viewModel.Allergies = await _context.Allergies.ToListAsync();
            return View(viewModel);
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
        #endregion


        #region SHOWDETAIL
        public IActionResult ShowDetails(int id)
        {
            Patient? patient = _context.Patients
                .Include(p => p.Allergies)
                .Include(p => p.Antecedents)
                .FirstOrDefault(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            var viewmodel = new PatientViewModel
            {
                Patient = patient,
                Antecedents = _context.Antecedents.ToList(),
                Allergies = _context.Allergies.ToList(),
                SelectedAntecedentIds = patient.Antecedents.Select(a => a.AntecedentId).ToList() ?? new List<int>(),
                SelectedAllergieIds = patient.Allergies.Select(a => a.AllergieId).ToList() ?? new List<int>()
            };

            return View(viewmodel);
        }
        //
        #endregion


    }
}