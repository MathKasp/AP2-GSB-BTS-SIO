using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace newEmpty.Controllers
{
    public class OrdonnanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Medecin> _userManager;

        public OrdonnanceController(ApplicationDbContext context, UserManager<Medecin> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]

        #region INDEX
        public async Task<IActionResult> Index()
        {
            string? medecinId = _userManager.GetUserId(User);
            List<Ordonnance> ordonnances = new List<Ordonnance>();

            ordonnances = await _context.Ordonnances
                .Include(o => o.Medecin)
                .Where(o => o.MedecinId == medecinId)
                .Include(o => o.Patient)
                .ToListAsync();

            ordonnances.OrderByDescending(o => o.Patient.Nom_p);

            return View(ordonnances);
        }
        #endregion

        #region ADD
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            string? MedecinId = _userManager.GetUserId(User);
            Medecin? med = _userManager.FindByIdAsync(MedecinId).Result;

            if (med == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            var viewModel = new OrdonnanceViewModel
            {
                Patients = await _context.Patients.ToListAsync(),
                Medicaments = await _context.Medicaments.ToListAsync(),
                SelectedMedicamentId = new List<int>(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(OrdonnanceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Patients = await _context.Patients.ToListAsync();
                viewModel.Medicaments = await _context.Medicaments.ToListAsync();
                return View(viewModel);
            }

            string MedecinID = _userManager.GetUserId(User);
            Medecin med = _userManager.FindByIdAsync(MedecinID).Result;

            var patient = await _context.Patients
                .Include(p => p.Antecedents)
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.PatientId == (int)viewModel.PatientId);
            if (patient == null)
                return NotFound();


            Ordonnance ordonnance = new Ordonnance
            {
                Posologie = viewModel.Posologie,
                Duree_traitement = viewModel.Duree_traitement,
                Instructions_specifique = viewModel.Instructions_specifique,
                MedecinId = MedecinID,
                Medecin = med,
                PatientId = (int)viewModel.PatientId,
                Patient = patient,
                Medicaments = new List<Medicament>()
            };

            if (viewModel.SelectedMedicamentId != null && viewModel.SelectedMedicamentId.Count > 0)
            {
                var selectedMedicament = await _context.Medicaments
                    .Where(a => viewModel.SelectedMedicamentId.Contains(a.MedicamentId))
                    .Include(m => m.Allergies)
                    .Include(m => m.Antecedents)
                    .ToListAsync();
                foreach (var medicament in selectedMedicament)
                {
                    ordonnance.Medicaments.Add(medicament);
                }
            }
            else
            {
                viewModel.Patients = await _context.Patients.ToListAsync();
                viewModel.Medicaments = await _context.Medicaments.ToListAsync();
                ModelState.AddModelError("", "Veuillez chosir au moins 1 médicament");
                return View(viewModel);
            }

            bool resultAdd = VerifyImpossibility(ordonnance);

            if (!resultAdd)
            {
                viewModel.Patients = await _context.Patients.ToListAsync();
                viewModel.Medicaments = await _context.Medicaments.ToListAsync();
                ModelState.AddModelError("", "Le patient ne peux pas avoir ces médicaments");
                return View(viewModel);
            }

            await _context.Ordonnances.AddAsync(ordonnance);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #region VERIFICATION DES CONTRES INDICATIONS
        public bool VerifyImpossibility(Ordonnance ordonnance)
        {
            List<Allergie> allergiesMedicaments = GetAllergiesMed(ordonnance);
            List<Antecedent> antecedentsMedicaments = GetAntecedentsMed(ordonnance);
            if (ordonnance.Patient == null)
            {
                throw new ArgumentNullException(nameof(ordonnance.Patient), "Une erreur imprévu sur le patient a été trouvé.");
            }
            List<Allergie> allergiesPatient = ordonnance.Patient.Allergies;
            foreach (Allergie allergieM in allergiesMedicaments)
            {
                foreach (Allergie allergieP in allergiesPatient)
                {
                    if (allergieM == allergieP)
                    {
                        return false;
                    }
                }
            }
            List<Antecedent> antecedentsPatient = ordonnance.Patient.Antecedents;
            foreach (Antecedent antecedentM in antecedentsMedicaments)
            {
                foreach (Antecedent antecedentP in antecedentsPatient)
                {
                    if (antecedentM == antecedentP)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #region GETALLERGIE
        public List<Allergie> GetAllergiesMed(Ordonnance ordonnance)
        {
            List<Allergie> allergies = new List<Allergie>();
            if (ordonnance == null)
            {
                throw new ArgumentNullException(nameof(ordonnance), "Une erreur imprévu sur l'ordonnace a été trouvé.");
            }

            if (ordonnance.Medicaments != null)
            {
                foreach (var medicament in ordonnance.Medicaments)
                {
                    if (medicament.Allergies != null)
                    {
                        foreach (var allergie in medicament.Allergies)
                            allergies.Add(allergie);
                    }
                }


            }
            return allergies;
        }
        #endregion

        #region GETANTECEDENT
        public List<Antecedent> GetAntecedentsMed(Ordonnance ordonnance)
        {
            List<Antecedent> antecedents = new List<Antecedent>();
            if (ordonnance == null)
            {
                throw new ArgumentNullException(nameof(ordonnance), "Une erreur imprévu sur l'ordonnace a été trouvé.");
            }

            if (ordonnance.Medicaments != null)
            {
                foreach (var medicament in ordonnance.Medicaments)
                {
                    if (medicament.Antecedents != null)
                    {
                        foreach (var antecedent in medicament.Antecedents)
                            antecedents.Add(antecedent);
                    }
                }
            }
            return antecedents;
        }
        #endregion

        #endregion

        #endregion

        #region EDIT
        public async Task<IActionResult> Edit(int id)
        {
            var ordonnance = await _context.Ordonnances
               .Include(o => o.Medicaments).ThenInclude(m => m.Allergies)
               .Include(o => o.Medicaments).ThenInclude(m => m.Antecedents)
               .Include(o => o.Patient).ThenInclude(p => p.Allergies)
               .Include(o => o.Patient).ThenInclude(p => p.Antecedents)
               .FirstOrDefaultAsync(o => o.OrdonnanceId == id);

            if (ordonnance == null)
                return NotFound();

            var viewModel = new OrdonnanceViewModel
            {
                OrdonnanceId = ordonnance.OrdonnanceId,
                Posologie = ordonnance.Posologie,
                Duree_traitement = ordonnance.Duree_traitement,
                Instructions_specifique = ordonnance.Instructions_specifique,
                PatientId = ordonnance.PatientId,
                Patient = ordonnance.Patient,
                Medicaments = await _context.Medicaments.ToListAsync(),
                Patients = await _context.Patients.ToListAsync(),
                SelectedMedicamentId = ordonnance.Medicaments.Select(m => m.MedicamentId).ToList() ?? new List<int>()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // si déconnecté il refuse l'action
        public async Task<IActionResult> Edit(int id, OrdonnanceViewModel viewModel)
        {
            if (id != viewModel.OrdonnanceId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                viewModel.Patients = await _context.Patients.ToListAsync();
                viewModel.Medicaments = await _context.Medicaments.ToListAsync();
                return View(viewModel);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var ordonnance = await _context.Ordonnances
                        .Include(o => o.Medicaments).ThenInclude(m => m.Allergies)
                        .Include(o => o.Medicaments).ThenInclude(m => m.Antecedents)
                        .Include(o => o.Patient).ThenInclude(p => p.Allergies)
                        .Include(o => o.Patient).ThenInclude(p => p.Antecedents)
                        .Include(o => o.Medecin)
                        .FirstOrDefaultAsync(o => o.OrdonnanceId == id);

                    if (ordonnance == null)
                    {
                        return NotFound();
                    }

                    var patient = await _context.Patients
                        .Include(p => p.Antecedents)
                        .Include(p => p.Allergies)
                        .FirstOrDefaultAsync(p => p.PatientId == (int)viewModel.PatientId);

                    ordonnance.Posologie = viewModel.Posologie;
                    ordonnance.Duree_traitement = viewModel.Duree_traitement;
                    ordonnance.Instructions_specifique = viewModel.Instructions_specifique;
                    ordonnance.PatientId = (int)viewModel.PatientId;
                    ordonnance.Patient = patient;

                    ordonnance.Medicaments.Clear();
                    if (viewModel.SelectedMedicamentId != null && viewModel.SelectedMedicamentId.Count > 0)
                    {
                        var selectedMedicament = await _context.Medicaments
                            .Where(a => viewModel.SelectedMedicamentId.Contains(a.MedicamentId))
                            .Include(m => m.Allergies)
                            .Include(m => m.Antecedents)
                            .ToListAsync();

                        foreach (var medicament in selectedMedicament)
                        {
                            ordonnance.Medicaments.Add(medicament);
                        }
                    }
                    else
                    {
                        viewModel.Patients = await _context.Patients.ToListAsync();
                        viewModel.Medicaments = await _context.Medicaments.ToListAsync();
                        ModelState.AddModelError("", "Veuillez choisir au moins 1 médicament");
                        return View(viewModel);
                    }

                    if (!VerifyImpossibility(ordonnance))
                    {
                        viewModel.Patients = await _context.Patients.ToListAsync();
                        viewModel.Medicaments = await _context.Medicaments.ToListAsync();
                        ModelState.AddModelError("", "Le patient ne peux pas avoir ces médicaments");
                        return View(viewModel);
                    }

                    _context.Entry(ordonnance).State = EntityState.Modified; // créer les entrées dans le contexte
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ThrowException();
                    if (!OrdonnanceExist((int)viewModel.OrdonnanceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ThrowException();
            viewModel.Patients = await _context.Patients.ToListAsync();
            viewModel.Medicaments = await _context.Medicaments.ToListAsync();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ThrowException()
        {
            throw new Exception("Une exception s'est produite, nous testons la page d'exception pour les développeurs.");
        }


        private bool OrdonnanceExist(int id)
        {
            return _context.Ordonnances.Any(e => e.OrdonnanceId == id);
        }
        #endregion

        #region SHOWDETAIL
        public IActionResult ShowDetails(int id)
        {
            Ordonnance? ordonnance = _context.Ordonnances
                .Include(p => p.Medicaments)
                .Include(p => p.Medecin)
                .Include(p => p.Patient)
                .FirstOrDefault(p => p.OrdonnanceId == id);

            if (ordonnance == null)
            {
                return NotFound();
            }

            var viewmodel = new OrdonnanceViewModel
            {
                OrdonnanceId = ordonnance.OrdonnanceId,
                Posologie = ordonnance.Posologie,
                Duree_traitement = ordonnance.Duree_traitement,
                Instructions_specifique = ordonnance.Instructions_specifique,
                PatientId = ordonnance.PatientId,
                Patient = ordonnance.Patient,
                Medecin = ordonnance.Medecin,
                Medicaments = _context.Medicaments.ToList(),
                SelectedMedicamentId = ordonnance.Medicaments.Select(a => a.MedicamentId).ToList() ?? new List<int>()
            };

            return View(viewmodel);
        }
        //
        #endregion

        #region REMOVE
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Ordonnance? ordonnance = _context.Ordonnances.FirstOrDefault(s => s.OrdonnanceId == id);
            return View(ordonnance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // si déconnecté il refuse l'action
        public async Task<IActionResult> RemoveConfirm(int OrdonnanceId)
        {
            List<Ordonnance> ordonnances = new List<Ordonnance>();
            ordonnances = _context.Ordonnances.ToList();

            Ordonnance? ordonnance = ordonnances.FirstOrDefault(s => s.OrdonnanceId == OrdonnanceId);

            if (ordonnance != null)
            {
                _context.Ordonnances.Remove(ordonnance);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        #endregion 
    }
}