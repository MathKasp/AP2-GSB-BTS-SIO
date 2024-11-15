using Microsoft.AspNetCore.Mvc;
using newEmpty.Models;
using NewEmpty.Data;
using Microsoft.EntityFrameworkCore;
using newEmpty.ViewModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;

namespace newEmpty.Controllers
{
    public class MedicamentController : Controller
    {

        private readonly ApplicationDbContext _context;

        // Controleur, injection de dependance
        public MedicamentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]

        #region INDEX
        public IActionResult Index()
        {
            List<Medicament> medicaments = new List<Medicament>();
            medicaments = _context.Medicaments.ToList();
            return View(medicaments);
        }
        #endregion


        #region ADD
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new MedicamentViewModel { };

            model.Antecedents = await _context.Antecedents.ToListAsync();
            model.Allergies = await _context.Allergies.ToListAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConfirmed(MedicamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Medicament.Allergies.Clear();
                if (model.SelectedAllergieIds != null)
                {
                    var selectedAllergies = await _context.Allergies
                        .Where(a => model.SelectedAllergieIds.Contains(a.AllergieId))
                        .ToListAsync();
                    foreach (var allergie in selectedAllergies)
                    {
                        model.Medicament.Allergies.Add(allergie);
                    }
                }
                model.Medicament.Antecedents.Clear();
                if (model.SelectedAntecedentIds != null)
                {
                    var selectedAntecedents = await _context.Antecedents
                        .Where(a => model.SelectedAntecedentIds.Contains(a.AntecedentId))
                        .ToListAsync();
                    foreach (var antecedent in selectedAntecedents)
                    {
                        model.Medicament.Antecedents.Add(antecedent);
                    }
                }
                _context.Medicaments.Add(model.Medicament);
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
            Medicament? medicament = _context.Medicaments.FirstOrDefault(s => s.MedicamentId == id);
            return View(medicament);
        }

        [HttpPost]
        public IActionResult RemoveConfirm(int MedicamentId)
        {
            List<Medicament> medicaments = new List<Medicament>();
            medicaments = _context.Medicaments.ToList();

            Medicament? medicament = medicaments.FirstOrDefault(s => s.MedicamentId == MedicamentId);

            if (medicament != null)
            {
                _context.Medicaments.Remove(medicament);
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
            var medicament = await _context.Medicaments
                .Include(p => p.Antecedents)
                .Include(p => p.Allergies)
                .FirstOrDefaultAsync(p => p.MedicamentId == id);

            if (medicament == null)
            {
                return NotFound();
            }

            // verifier si les allergies et/ou les antecedents sont nulls
            var viewModel = new MedicamentViewModel
            {
                Medicament = medicament,
                Antecedents = await _context.Antecedents.ToListAsync(),
                Allergies = await _context.Allergies.ToListAsync(),
                SelectedAntecedentIds = medicament.Antecedents.Select(a => a.AntecedentId).ToList() ?? new List<int>(),
                SelectedAllergieIds = medicament.Allergies.Select(a => a.AllergieId).ToList() ?? new List<int>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Expliquer 
        public async Task<IActionResult> Edit(int id, MedicamentViewModel viewModel)
        {
            if (id != viewModel.Medicament.MedicamentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var medicament = await _context.Medicaments
                        .Include(p => p.Antecedents)
                        .Include(p => p.Allergies)
                        .FirstOrDefaultAsync(p => p.MedicamentId == id);

                    if (medicament == null)
                    {
                        return NotFound();
                    }

                    medicament.Nom_med = viewModel.Medicament.Nom_med;
                    medicament.Contre_indication = viewModel.Medicament.Contre_indication;

                    medicament.Allergies.Clear();
                    if (viewModel.SelectedAllergieIds != null)
                    {
                        var selectedAllergies = await _context.Allergies
                            .Where(a => viewModel.SelectedAllergieIds.Contains(a.AllergieId))
                            .ToListAsync();
                        foreach (var allergie in selectedAllergies)
                        {
                            medicament.Allergies.Add(allergie);
                        }
                    }

                    medicament.Antecedents.Clear();
                    if (viewModel.SelectedAntecedentIds != null)
                    {
                        var selectedAntecedents = await _context.Antecedents
                            .Where(a => viewModel.SelectedAntecedentIds.Contains(a.AntecedentId))
                            .ToListAsync();
                        foreach (var antecedent in selectedAntecedents)
                        {
                            medicament.Antecedents.Add(antecedent);
                        }
                    }
                    _context.Entry(medicament).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentExists(viewModel.Medicament.MedicamentId))
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

        private bool MedicamentExists(int id)
        {
            return _context.Medicaments.Any(e => e.MedicamentId == id);
        }
        #endregion


        #region SHOWDETAIL
        public IActionResult ShowDetails(int id)
        {
            Medicament? medicament = _context.Medicaments
                .Include(p => p.Allergies)
                .Include(p => p.Antecedents)
                .FirstOrDefault(p => p.MedicamentId == id);

            if (medicament == null)
            {
                return NotFound();
            }

            var viewmodel = new MedicamentViewModel
            {
                Medicament = medicament,
                Antecedents = _context.Antecedents.ToList(),
                Allergies = _context.Allergies.ToList(),
                SelectedAntecedentIds = medicament.Antecedents.Select(a => a.AntecedentId).ToList() ?? new List<int>(),
                SelectedAllergieIds = medicament.Allergies.Select(a => a.AllergieId).ToList() ?? new List<int>()
            };

            return View(viewmodel);
        }
        //
        #endregion


    }
}